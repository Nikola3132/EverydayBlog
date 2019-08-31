namespace EveryDayBlog.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class LoginWith2faModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private const string TwoFactorLogMsg = "User with ID '{0}' logged in with 2fa.";
        private const string LockedOutLogErrorMsg = "User with ID '{0}' account locked out.";
        private const string InvalidAuthenticationLogMsg = "Invalid authenticator code entered for user with ID '{0}'.";
        private const string InvalidAuthCode = "Invalid authenticator code.";
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LoginWith2faModel> logger;

        public LoginWith2faModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginWith2faModel> logger)
        {
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public PageHeaderViewModel PageHeader { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await this.signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException(message: $"Unable to load two-factor authentication user.");
            }

            this.ReturnUrl = returnUrl;
            this.RememberMe = rememberMe;

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            returnUrl = returnUrl ?? this.Url.Content("~/");

            var user = await this.signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException(message: $"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = this.Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await this.signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, this.Input.RememberMachine);

            if (result.Succeeded)
            {
                this.logger.LogInformation(TwoFactorLogMsg, user.Id);
                return this.LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                this.logger.LogWarning(LockedOutLogErrorMsg, user.Id);
                return this.RedirectToPage("./Lockout");
            }
            else
            {
                this.logger.LogWarning(InvalidAuthenticationLogMsg, user.Id);
                this.ModelState.AddModelError(string.Empty, InvalidAuthCode);
                return this.Page();
            }
        }

        public class InputModel
        {
            private const string TwoFactorCodeLenghtErrorMsg = "The {0} must be at least {2} and at max {1} characters long.";
            private const string AuthDisplay = "Authenticator code";
            private const string RememberMeDisplay = "Remember this machine";

            [Required]
            [StringLength(7, ErrorMessage = TwoFactorCodeLenghtErrorMsg, MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = AuthDisplay)]
            public string TwoFactorCode { get; set; }

            [Display(Name = RememberMeDisplay)]
            public bool RememberMachine { get; set; }
        }
    }
}
