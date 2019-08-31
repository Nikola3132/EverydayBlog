namespace EveryDayBlog.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

#pragma warning disable SA1649 // File name should match first type name
    public class GenerateRecoveryCodesModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private const string RecoveryCodeCreatedMsg = "You have generated new recovery codes.";
        private const string CreatedUserLogMsg = "User with ID '{0}' has generated new 2FA recovery codes.";
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<GenerateRecoveryCodesModel> logger;

        public GenerateRecoveryCodesModel(
            UserManager<ApplicationUser> userManager,
            ILogger<GenerateRecoveryCodesModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        [TempData]
        public string[] RecoveryCodes { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound(value: $"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var isTwoFactorEnabled = await this.userManager.GetTwoFactorEnabledAsync(user);
            if (!isTwoFactorEnabled)
            {
                var userId = await this.userManager.GetUserIdAsync(user);
                throw new InvalidOperationException(message: $"Cannot generate recovery codes for user with ID '{userId}' because they do not have 2FA enabled.");
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound(value: $"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var isTwoFactorEnabled = await this.userManager.GetTwoFactorEnabledAsync(user);
            var userId = await this.userManager.GetUserIdAsync(user);
            if (!isTwoFactorEnabled)
            {
                throw new InvalidOperationException(message: $"Cannot generate recovery codes for user with ID '{userId}' as they do not have 2FA enabled.");
            }

            var recoveryCodes = await this.userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            this.RecoveryCodes = recoveryCodes.ToArray();

            this.logger.LogInformation(CreatedUserLogMsg, userId);
            this.StatusMessage = RecoveryCodeCreatedMsg;
            return this.RedirectToPage("./ShowRecoveryCodes");
        }
    }
}
