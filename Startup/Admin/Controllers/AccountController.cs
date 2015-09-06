using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Admin.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Access;
using Access.Repositories;
using Access.Models;
using System.Data.Entity;
using Access.Extensions;

namespace Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return User.IsInRole("Admin") ? RedirectToAction("Menu", "RootAdmin"):  RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Usuario o Constraseña Invalido.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Codigo Invalido");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Canchas");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);


            // Agregar custom Claims 
            if (result == SignInStatus.Success)
            {
                var user = await UserManager.FindByEmailAsync(loginInfo.Email);

                 await CreateExternalClaimsAsync(user, loginInfo);
            }
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                     
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        await CreateExternalClaimsAsync(user, info);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        private async Task CreateExternalClaimsAsync(ApplicationUser user ,ExternalLoginInfo info)
        {
            var existingClaims = await UserManager.GetClaimsAsync(user.Id);
            if (existingClaims != null && !existingClaims.Any())
            {
                var tasks = info.ExternalIdentity
                    .Claims.Select(c => new Task(() => UserManager.AddClaimAsync(user.Id, c))).ToList();

                tasks.ForEach(t => t.RunSynchronously());
            }
        }
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Canchas");
        }

        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Canchas");
        }

        public  ActionResult GetUserClaims()
        { 
             
            return View();
        }

        
        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region methods account management

        public IdentityManagerService IdentityManagerService
        {
            get
            {
                return new IdentityManagerService(HttpContext.GetOwinContext().Get<ApplicationDbContext>())
                {

                };
            }
        }

        // Helper Methods 
        // JsonReuslt /ViewResult For Accounts Picker
        #region Account Selector Results
        /// <summary>
        ///Returns All Roles List
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetRoles()
        {
            var roles = await IdentityManagerService.GetRolesDataAsync();

            return Json(roles, JsonRequestBehavior.AllowGet);
        }

        public async  Task<JsonResult> GetCenters()
        {
            var centers = await CenterRepository.Centers()
                          .Select(c=> new SelectListModel<string>() { Id = c.Id.ToString(), Text = c.Name })
                          .ToListAsync();

            return Json(centers, JsonRequestBehavior.AllowGet);
        }
 
        public async Task<JsonResult> GetUserNamesForSchedule(string text)
        {
           
            var users = await IdentityManagerService.GetUsersAsync(
                new FilterOptionModel(ClaimsPrincipal.Current.CenterId())
                {
                    keywords = text,
            
                },Context,onlyUsers:true);

            var model = users.ToIdentityUserViewModel()
                .Select(u =>
                new AutoCompleteModel
                {
                    Id = u.Id,
                    Name = string.Format("{0} - {1}", u.FirstName, u.Email)
                }).ToList() ;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

      
      
        [HttpPost]

        public async Task<ActionResult> GetIdentityPreview(string id)
        {
            var user = await IdentityManagerService.GetUserAsync(id);

            return View(@"~/Views/Account/Partials/IdentityPreview.cshtml", await user.ToIdentityUserViewModelAsync(IdentityManagerService));
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> GetPartialLogin(string userid)
        {
            var user = await IdentityManagerService.GetUserAsync(userid);

            return View(@"~/Views/Account/Partials/PartialLogin.cshtml", await user.ToIdentityUserViewModelAsync(IdentityManagerService));
        }




        #endregion Accoutn Selector Results

        private CenterRepository CenterRepository
        {
            get
            {
                return new CenterRepository()
                {
                    Context = Context,
                };
            }
        }

        private AccessContext Context
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AccessContext>();
            }
        }

       

        [Authorize(Roles = "Admin,Manager")]
        //[AllowAnonymous]
        public async Task<ActionResult> AccountMangement()
        {
            
            var filter = new FilterOptionModel(ClaimsPrincipal.Current.CenterId()) {
                Limit = 12,
            };
            var users = await IdentityManagerService.GetUsersAsync(filter, Context);
            ViewBag.PageLimit = await IdentityManagerService.GetPageLimit(filter, Context);

            var model = users.ToIdentityUserViewModel();

            return View(model);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> SearchAccounts(FilterOptionModel filter)
        {
            if(!filter.centerid.HasValue && User.IsInRole("Manager") ) filter.centerid = ClaimsPrincipal.Current.CenterId();

            var users = await IdentityManagerService.GetUsersAsync(filter,Context);

            var model =  users.ToIdentityUserViewModel();

            ViewBag.PageLimit = await IdentityManagerService.GetPageLimit(filter,Context);

            return PartialView(@"~/Views/Account/Partials/UserManagementGrid.cshtml", model);
        }

        //[Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<ActionResult> EditApplicationUser(string id = "")
        {
            ViewBag.Roles = new SelectList(await IdentityManagerService.GetRolesAsync());
            var model = await IdentityManagerService.GetUserAsync(id) ?? new ApplicationUser();
            
            return View(@"~/Views/Account/Partials/EditApplicationUser.cshtml", await model.ToIdentityUserViewModelAsync(IdentityManagerService));
        }


        //[Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<ActionResult> UpdateApplicationUser(IdentityUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                await IdentityManagerService.InsertOrUpdate(model, UserManager);
                //await CenterRepository.UpdateEmployeeCenter(model.Id, model.CenterId,new Guid(User.Identity.GetUserId()));
              
                return RedirectToAction("AccountMangement");
            }
            ViewBag.Roles = new SelectList(await IdentityManagerService.GetRolesAsync());
            return View("~/Views/Account/EditApplicationUser", model);
        }
        [HttpPost]
        //[Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> ApplicationUserDetails(string id)
        {
            var model = await IdentityManagerService.GetUserAsync(id) ?? new ApplicationUser();
            return View(await model.ToIdentityUserViewModelAsync(IdentityManagerService));
        }

        //public async Task<JsonResult> GetApplicationUsers()
        //{
        //    var users = await IdentityManagerService.GetApplicationUsers();

        //    var model = users.Select(u => u.ToIdentityUserViewModel()).ToList();

        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        

        #endregion

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return User.IsInRole("admin") ? RedirectToAction("RootAdmin", "RootAdmin") : RedirectToAction("Index", "Canchas");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}