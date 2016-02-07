
using Access;
using Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Config
{

      
        public class EmailService : IIdentityMessageService
        {
            public Task SendAsync(IdentityMessage message)
            {
                // Plug in your email service here to send an email.
                // Plug in your email service here to send an email.
                // Credentials:
                var credentialUserName = "enlacancha@enlacancha.net";
                var sentFrom = "enlacancha@enlacancha.net";
                var pwd = "12345678";

                try
                {
                    // Configure the client:
                    System.Net.Mail.SmtpClient client =
                        new System.Net.Mail.SmtpClient("smtpout.secureserver.net");

                    client.Port = 80;
                    client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;

                    // Create the credentials:
                    System.Net.NetworkCredential credentials =
                        new System.Net.NetworkCredential(credentialUserName, pwd);

                    client.EnableSsl = false;
                    client.Credentials = credentials;

                    var view = AlternateView.CreateAlternateViewFromString(message.Body, null, "text/html");


                    // Create the message:
                    var mail =
                        new System.Net.Mail.MailMessage(sentFrom, message.Destination);

                    mail.Subject = message.Subject;
                    mail.AlternateViews.Add(view);
                    mail.IsBodyHtml = true;

                    // Send:
                    return client.SendMailAsync(mail);
                }
                catch (Exception ex)
                {
                    //throw ex;
                    return Task.FromResult(0);
                }
            }
        }

        public class SmsService : IIdentityMessageService
        {
            public Task SendAsync(IdentityMessage message)
            {
                // Plug in your SMS service here to send a text message.
                return Task.FromResult(0);
            }
        }

        // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
        public class ApplicationUserManager : UserManager<ApplicationUser>
        {
            public ApplicationUserManager(IUserStore<ApplicationUser> store)
                : base(store)
            {
            }

            public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
            {
                var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<AccessContext>()));
                // Configure validation logic for usernames
                manager.UserValidator = new UserValidator<ApplicationUser>(manager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                };

                // Configure validation logic for passwords
                manager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false,
                };

                // Configure user lockout defaults
                manager.UserLockoutEnabledByDefault = true;
                manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
                manager.MaxFailedAccessAttemptsBeforeLockout = 5;

                // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
                // You can write your own provider and plug it in here.
                manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
                {
                    MessageFormat = "Your security code is {0}"
                });
                manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
                {
                    Subject = "Codigo De Seguridad",
                    BodyFormat = "Tu Codigo de Seguridad es {0}"
                });
                manager.EmailService = new EmailService();
                manager.SmsService = new SmsService();
                var dataProtectionProvider = options.DataProtectionProvider;
                if (dataProtectionProvider != null)
                {
                    manager.UserTokenProvider =
                        new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
                }
                return manager;
            }

            public static async Task<bool> SeedRoles()
            {

                var appContext = AccessContext.Create();
                var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(appContext));


                if (!await appContext.Roles.AnyAsync(role => role.Name == "Admin"))
                {
                    appContext.Roles.Add(new IdentityRole("Admin"));
                    var user = new ApplicationUser
                    {
                        Email = "admin@yopmail.com",
                        UserName = "admin@yopmail.com",

                    };
                    await manager.CreateAsync(user, "1234567");
                    await manager.AddToRoleAsync(user.Id, "Admin");
                }


                manager.Dispose();

                appContext.Dispose();

                return await Task.FromResult(true);
            }
        }

        // Configure the application sign-in manager which is used in this application.
        public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
        {
            public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
                : base(userManager, authenticationManager)
            {
            }

        public override async Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            var identity = await user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);

            identity.AddClaim( new Claim("facebookUserPicture", user.ProfilePicture));


            return identity;
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
            {
                return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
            }
        }
     
}
