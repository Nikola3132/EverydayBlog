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
    public class LoginWithRecoveryCodeModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private const string UserRecoveryCodeLog = "User with ID '{0}' logged in with a recovery code.";
        private const string LogOutLogMsg = "User with ID '{0}' account locked out.";
        private const string InvalidCodeLogMsg = "Invalid recovery code entered for user with ID '{0}' ";
        private const string InvalidRecoveryCodeMsg = "Invalid recovery code entered.";
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LoginWithRecoveryCodeModel> logger;

        public LoginWithRecoveryCodeModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginWithRecoveryCodeModel> logger)
        {
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public PageHeaderViewModel PageHeader { get; set; }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await this.signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException(message: $"Unable to load two-factor authentication user.");
            }

            this.ReturnUrl = returnUrl;

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this.signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException(message: $"Unable to load two-factor authentication user.");
            }

            var recoveryCode = this.Input.RecoveryCode.Replace(" ", string.Empty);

            var result = await this.signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                this.logger.LogInformation(UserRecoveryCodeLog, user.Id);
                return this.LocalRedirect(returnUrl ?? this.Url.Content("~/"));
            }

            if (result.IsLockedOut)
            {
                this.logger.LogWarning(LogOutLogMsg, user.Id);
                return this.RedirectToPage("./Lockout");
            }
            else
            {
                this.logger.LogWarning(InvalidCodeLogMsg, user.Id);
                this.ModelState.AddModelError(string.Empty, InvalidRecoveryCodeMsg);
                return this.Page();
            }
        }

        public class InputModel
        {
            [BindProperty]
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Recovery Code")]
            public string RecoveryCode { get; set; }
        }
    }
}
