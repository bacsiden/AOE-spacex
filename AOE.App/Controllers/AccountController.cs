using AOE.App.Models.AccountViewModels;
using AOE.Application.Models.Framework;
using AOE.Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AOE.App.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IUserService userService,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _userService = userService;
            _logger = logger;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = "/")
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Lockout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(AccountController.Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            string redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else // In case user does not exists, create user
            {
                string username = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.NewGuid().ToString().Remove('-');
                string name = info.Principal.FindFirstValue(ClaimTypes.Name) ?? username;
                string email = info.Principal.FindFirstValue(ClaimTypes.Email) ?? "";
                string dob = info.Principal.FindFirstValue(ClaimTypes.DateOfBirth);
                string gender = info.Principal.FindFirstValue(ClaimTypes.Gender);
                string avatar = "";
                if (info.LoginProvider == "Google")
                {
                    avatar = info.Principal.FindFirstValue("image");
                    // avatar = $"https://www.googleapis.com/plus/v1/people/{info.ProviderKey}?fields=image&key={googleApiKey}";
                }

                ApplicationUser user = new ApplicationUser { UserName = username, Email = email, Name = name, Avatar = avatar, Gender = gender };
                IdentityResult result1 = await _userManager.CreateAsync(user);
                if (result1.Succeeded)
                {
                    result1 = await _userManager.AddLoginAsync(user, info);
                    if (result1.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }

                return RedirectToLocal(returnUrl);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                IdentityResult result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        private List<string> GetMessages(ModelStateDictionary modelState)
        {
            return modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => x.ErrorMessage))
                    .ToList();
        }

        #endregion

        public LayoutInfoModel GetLayoutInfoModel()
        {
            return new LayoutInfoModel
            {
                EmailCount = 3
            };
        }

        [TempData]
        public string StatusMessage { get; set; }
    }
}
