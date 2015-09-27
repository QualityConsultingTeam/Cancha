using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using Access;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Newtonsoft.Json.Linq;
using Owin;
using Admin.Models;
using Identity.Models;
using Identity.Context;
using Identity.Config;

namespace Admin
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

            
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<AccessContext>(AccessContext.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieName = "enlacanchaauthcookie",
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromSeconds(5),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");
            var facebookOptions = new FacebookAuthenticationOptions();
            facebookOptions.AppId = ConfigurationManager.AppSettings["FacebookApiId"];// "1376699832655793";
            facebookOptions.AppSecret = ConfigurationManager.AppSettings["facebookSecret"];//"03a61edc931982826f611b5d76786d53";
            facebookOptions.Scope.Add("email");
      

            facebookOptions.Provider = new FacebookAuthenticationProvider()
            {
                //  OnAuthenticated = (facebookContext) =>
                //{
                //    // Save every additional claim we can find in the user
                //    //  foreach (JProperty property in facebookContext.User.Children())
                //    //{
                //    //    var claimType = string.Format("urn:facebook:{0}", property.Name);
                //    //    string claimValue = (string)property.Value;
                //    //    if (!facebookContext.Identity.HasClaim(claimType, claimValue))
                //    //        facebookContext.Identity.AddClaim(new Claim(claimType, claimValue,
                //    //              "http://www.w3.org/2001/XMLSchema#string", "External"));

                //    //} 



                //    return Task.FromResult(0);

                //}
                OnAuthenticated =   context =>
                {
                    foreach (var x in context.User)
                    {
                        context.Identity.AddClaim(new Claim(x.Key, x.Value.ToString()));
                    }
                    //Get the access token from FB and store it in the database and use FacebookC# SDK to get more information about the user
                    context.Identity.AddClaim(new Claim("FacebookAccessToken", context.AccessToken));

                    return Task.FromResult(true);
                }


            };

            app.UseFacebookAuthentication(facebookOptions);
            //app.UseFacebookAuthentication(
            //    appId: "1376699832655793", //"355447304638921",
            //    appSecret: "03a61edc931982826f611b5d76786d53"); // "2400c53a5cce19396d3ccf4c254c0ba9");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}