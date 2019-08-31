namespace EveryDayBlog.Web.Areas.Identity.Pages.Account.Manage
{
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

#pragma warning disable SA1649 // File name should match first type name
    public class ResetAuthenticatorModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private const string AppKeyLogMsg = "User with ID '{0}' has reset their authentication app key.";
        private const string ResetAuthAppKeyMsg = "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.";
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<ResetAuthenticatorModel> logger;

        public ResetAuthenticatorModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ResetAuthenticatorModel> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public PageHeaderViewModel PageHeader { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound(value: $"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
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

            await this.userManager.SetTwoFactorEnabledAsync(user, false);
            await this.userManager.ResetAuthenticatorKeyAsync(user);
            this.logger.LogInformation(AppKeyLogMsg, user.Id);

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = ResetAuthAppKeyMsg;

            return this.RedirectToPage("./EnableAuthenticator");
        }
    }
}
