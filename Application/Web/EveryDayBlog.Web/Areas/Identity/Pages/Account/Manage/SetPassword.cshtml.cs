namespace EveryDayBlog.Web.Areas.Identity.Pages.Account.Manage
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

#pragma warning disable SA1649 // File name should match first type name
    public class SetPasswordModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private const string PasswordSetMsg = "Your password has been set.";
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public SetPasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public PageHeaderViewModel PageHeader { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound(value: $"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var hasPassword = await this.userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return this.RedirectToPage("./ChangePassword");
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound(value: $"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var addPasswordResult = await this.userManager.AddPasswordAsync(user, this.Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                return this.Page();
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = PasswordSetMsg;

            return this.RedirectToPage();
        }

        public class InputModel
        {
            private const string ConfrimPasswordErrorMsg = "The new password and confirmation password do not match.";
            private const string PasswordLenghtErrorMsg = "The {0} must be at least {2} and at max {1} characters long.";

            [Required]
            [StringLength(100, ErrorMessage = PasswordLenghtErrorMsg, MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = ConfrimPasswordErrorMsg)]
            public string ConfirmPassword { get; set; }
        }
    }
}
