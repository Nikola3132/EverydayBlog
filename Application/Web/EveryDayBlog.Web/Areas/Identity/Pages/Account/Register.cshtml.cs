namespace EveryDayBlog.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using EveryDayBlog.Common;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Web.CustomAttributes;
    using EveryDayBlog.Web.ModelBinders;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGet(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
            }

            if (this.User.Identity.IsAuthenticated)
            {
                this.Response.Redirect("/Home/Error");
            }

            returnUrl = returnUrl ?? this.Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(List<IFormFile> files, string returnUrl = null)
        {

            returnUrl = returnUrl ?? this.Url.Content("~/");
            if (!this.ModelState.IsValid)
            {
                // If we got this far, something failed, redisplay form
                return this.Page();
            }

            var imgForDb = this.Input.Image;

            var user = new ApplicationUser
            {
                UserName = this.Input.Email
                ,Email = this.Input.Email
                ,CreatedOn = DateTime.UtcNow
                ,Description = this.Input.Description
                ,FirstName = this.Input.FirstName
                ,LastName = this.Input.LastName
            };

            if (imgForDb != null)
            {
                user.Image = this.Input.Image;
            }

            var result = await this.userManager.CreateAsync(user, this.Input.Password);
            if (result.Succeeded)
            {
                this.logger.LogInformation("User created a new account with password.");

                //Sending an email

                //var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                //var callbackUrl = this.Url.Page(
                //    "/Account/ConfirmEmail",
                //    pageHandler: null,
                //    values: new { userId = user.Id, code = code },
                //    protocol: this.Request.Scheme);

                //await this.emailSender.SendEmailAsync(
                //    this.Input.Email,
                //    "Confirm your email",
                //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                await this.signInManager.SignInAsync(user, isPersistent: false);
                return this.LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }

            return this.Page();
        }

        public class InputModel
        {
            private const string DescriptionErrorMsg = "Your {0} cannot be with lower than {0} symbols";
            private const string NameErrorMsg = "Your {0} cannot be with more than {1} and lower than {2} symbols";
            private const string PasswordErrorMsg = "The {0} must be at least {2} and max {1} characters long.";

            // I cant put the string (jpg,jpeg,png,pdf) in the const dinamically because its const and asp.net core throws FormatException
            private const string ImgExtsErrorMsg = "Your extension should be one of the following: jpg,jpeg,png,pdf";



            [Required]
            [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = NameErrorMsg)]
            [DataType(DataType.Text)]
            [Display(Name = "first name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = NameErrorMsg)]
            [DataType(DataType.Text)]
            [Display(Name = "last name")]
            public string LastName { get; set; }

            [MinLength(10, ErrorMessage = DescriptionErrorMsg)]
            [DataType(DataType.MultilineText)]
            [Display(Name = "description")]
            public string Description { get; set; }

            [ModelBinder(typeof(FileToImageModelBinder))]
            [DataType(DataType.Upload)]
            [ImageExtensions(GlobalConstants.AllowedImageExtensions, ErrorMessage= ImgExtsErrorMsg)]
            public Image Image { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = PasswordErrorMsg, MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}
