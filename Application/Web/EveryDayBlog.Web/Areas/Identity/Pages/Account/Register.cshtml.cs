namespace EveryDayBlog.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using AutoMapper;
    using EveryDayBlog.Common;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services;
    using EveryDayBlog.Services.Data;
    using EveryDayBlog.Services.Mapping;
    using EveryDayBlog.Services.Messaging;
    using EveryDayBlog.Web.Infrastructure.CustomAttributes;
    using EveryDayBlog.Web.Infrastructure.Extensions;
    using EveryDayBlog.Web.Infrastructure.ModelBinders;
    using EveryDayBlog.Web.Infrastructure.Models;
    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Logging;
    using MimeKit;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private const string SuccessfullyCreatedUserLog = "User created a new account with password.";
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailService emailService;
        private readonly IUsersService usersService;
        private readonly IPageHeaderService pageHeaderService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailService emailService,
            IUsersService usersService,
            IPageHeaderService pageHeaderService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailService = emailService;
            this.usersService = usersService;
            this.pageHeaderService = pageHeaderService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public PageHeaderViewModel PageHeader { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGet(/*string returnUrl = null*/)
        {
            this.PageHeader = await this.GetPageHeaderAsync();

            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
            }

            if (this.User.Identity.IsAuthenticated)
            {
                this.Response.Redirect("/Home/Error");
            }

            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(List<IFormFile> files)
        {
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (!this.ModelState.IsValid)
            {
                // If we got this far, something failed, redisplay form
                return this.Page();
            }

            var user = new ApplicationUser
            {
                UserName = this.Input.Email,
                Email = this.Input.Email,
                CreatedOn = DateTime.UtcNow,
                Description = this.Input.Description,
                FirstName = this.Input.FirstName,
                LastName = this.Input.LastName,
                CountryCode = this.Input.CountryCode,
            };
            var result = await this.userManager.CreateAsync(user, this.Input.Password);

            if (this.Input.Image != null)
            {
               await this.usersService.AddUserImageAsync(this.Input.Image, user.UserName);
            }

            var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = this.Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = user.Id, code = code },
                protocol: this.Request.Scheme);

            await this.emailService.SendEmailToUserAsync(callbackUrl, this.Input.Email);

            if (result.Succeeded)
            {
                this.logger.LogInformation(SuccessfullyCreatedUserLog);

                this.TempData.Clear();

                TempDataExtensions.Put<EmailViewModel>(this.TempData, "EmailOptions", new EmailViewModel { Email = this.Input.Email, CallbackUrl = callbackUrl });

                return this.RedirectToPage("VerifyEmail");
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }

            return this.Page();
        }

        private async Task<PageHeaderViewModel> GetPageHeaderAsync()
        {
            var pageHeaders = await this.pageHeaderService.GetPageHeadersByPageIndicatorAsync<PageHeaderViewModel>(GlobalConstants.Registration);
            var pageHeader = pageHeaders.FirstOrDefault();

            return pageHeader;
        }

        public class InputModel : IMapTo<ApplicationUser>
        {
            private const string DescriptionErrorMsg = "Your {0} cannot be with lower than {1} symbols";
            private const string NameErrorMsg = "Your {0} cannot be with more than {1} and lower than {2} symbols";
            private const string PasswordErrorMsg = "The {0} must be at least {2} and max {1} characters long.";

            // I cant put the string (jpg,jpeg,png,pdf) in the const dinamically because its const and asp.net core throws FormatException
            private const string ImgExtsErrorMsg = "Your {0} extension should be one of the following: jpg,jpeg,png,pdf";
            private const string NameRegexErrorMsg = "You cannot have more than one capital letter, not any other symbols except Latin alphabets";

            private const string CountryCodeRequredErrorMsg = "You are obligated to select your country";
            private const string ConfirmPasswordErrorMsg = "The password and confirmation password do not match.";

            [Required]
            [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = NameErrorMsg)]
            [DataType(DataType.Text)]
            [RegularExpression("^[A-Z][a-z]+$", ErrorMessage = NameRegexErrorMsg)]
            [Display(Name = "first name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = NameErrorMsg)]
            [DataType(DataType.Text)]
            [RegularExpression("^[A-Z][a-z]+$", ErrorMessage = NameRegexErrorMsg)]
            [Display(Name = "last name")]
            public string LastName { get; set; }

            [MinLength(10, ErrorMessage = DescriptionErrorMsg)]
            [DataType(DataType.MultilineText)]
            [Display(Name = "description")]
            public string Description { get; set; }

            [ModelBinder(typeof(FileToImageModelBinder))]
            [DataType(DataType.Upload)]
            [Display(Name = "image")]
            [ImageExtensions(GlobalConstants.AllowedImageExtensions, ErrorMessage= ImgExtsErrorMsg)]
            public ImageInputModel Image { get; set; }

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
            [Compare("Password", ErrorMessage = ConfirmPasswordErrorMsg)]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = CountryCodeRequredErrorMsg)]
            public string CountryCode { get; set; }
        }
    }
}
