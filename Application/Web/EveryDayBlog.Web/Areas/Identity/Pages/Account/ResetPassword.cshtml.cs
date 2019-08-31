namespace EveryDayBlog.Web.Areas.Identity.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class ResetPasswordModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private const string Error = "A code must be supplied for password reset.";
        private readonly UserManager<ApplicationUser> userManager;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public PageHeaderViewModel PageHeader { get; set; }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return this.BadRequest(Error);
            }
            else
            {
                this.Input = new InputModel
                {
                    Code = code,
                };
                return this.Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this.userManager.FindByEmailAsync(this.Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return this.RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await this.userManager.ResetPasswordAsync(user, this.Input.Code, this.Input.Password);
            if (result.Succeeded)
            {
                return this.RedirectToPage("./Login");
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }

            return this.Page();
        }

        public class InputModel
        {
            private const string PasswordErrorMsg = "The {0} must be at least {2} and at max {1} characters long.";
            private const string DisplayPasswordName = "Confirm password";
            private const string PasswordConfirmErrorMsg = "The password and confirmation password do not match.";

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = PasswordErrorMsg, MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = DisplayPasswordName)]
            [Compare("Password", ErrorMessage = PasswordConfirmErrorMsg)]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }
    }
}
