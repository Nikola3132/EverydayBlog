namespace EveryDayBlog.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using EveryDayBlog.Common;
    using EveryDayBlog.Data.Common.Repositories;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Data;
    using EveryDayBlog.Web.Infrastructure.CustomAttributes;
    using EveryDayBlog.Web.Infrastructure.Extensions;
    using EveryDayBlog.Web.Infrastructure.ModelBinders;
    using EveryDayBlog.Web.Infrastructure.Models;
    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Caching.Distributed;

    public class IndexModel : PageModel
    {
        private const string Key = "EmailOptions";
        private const string UpdateEmailErrorMsg = "Your email won't be updated until you have confirm it in your mail!";
        private const string ProfileUpdateMsg = "Your profile has been updated";
        private const string EmailTemplateTitle = "Confirm your email";
        private const string InfoMsgEmailSended = "Verification email sent. Please check your email.";
        private const string ExistsEmailErrorMsg = "This email is already taken!";

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IEmailService emailService;
        private readonly IDistributedCache distributedCache;
        private readonly IDeletableEntityRepository<ApplicationUser> efRepository;
        private readonly IUsersService usersService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IEmailService emailService,
            IDistributedCache distributedCache,
            IDeletableEntityRepository<ApplicationUser> efRepository,
            IUsersService usersService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.emailService = emailService;
            this.distributedCache = distributedCache;
            this.efRepository = efRepository;
            this.usersService = usersService;
        }

        public string Username { get; set; }

        public PageHeaderViewModel PageHeader { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound(value: $"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            string cloudinaryUrl = await this.usersService.GetUserImageIfExistsAsync(this.User.Identity.Name);

            var userName = await this.userManager.GetUserNameAsync(user);
            var email = await this.userManager.GetEmailAsync(user);

            this.Username = userName;

            this.Input = new InputModel
            {
                Email = email,
                Country = user.CountryCode,
                Description = user.Description,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageCloudUrl = cloudinaryUrl,
                Profession = user.Profession,
            };

            this.IsEmailConfirmed = await this.userManager.IsEmailConfirmedAsync(user);

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                string cloudinaryUrl = await this.usersService.GetUserImageIfExistsAsync(this.User.Identity.Name);
                this.Input.ImageCloudUrl = cloudinaryUrl;
                return this.Page();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound(value: $"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var email = await this.userManager.GetEmailAsync(user);

            if (this.Input.Email != email)
            {
                var userExists = await this.userManager.FindByEmailAsync(this.Input.Email);

                if (userExists != null)
                {
                    this.TempData.Clear();

                    this.TempData["alert"] = ExistsEmailErrorMsg;
                }
                else
                {
                    var callbackUrl = this.Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { userId = user.Id },
                    protocol: this.Request.Scheme);

                    await this.emailService.SendEmailToUserAsync(callbackUrl, this.Input.Email);

                    this.TempData.Clear();
                    TempDataExtensions.Put<EmailViewModel>(this.TempData, Key, new EmailViewModel { Email = this.Input.Email, CallbackUrl = callbackUrl });

                    string distributedCacheKey = email;

                    await this.distributedCache.SetStringAsync(user.Email, this.Input.Email);

                    this.TempData["alert"] = UpdateEmailErrorMsg;
                }
            }

            user.CountryCode = this.Input.Country;
            user.ModifiedOn = DateTime.UtcNow;
            user.Profession = this.Input.Profession;
            user.FirstName = this.Input.FirstName;
            user.LastName = this.Input.LastName;
            user.Description = this.Input.Description;

            if (this.Input.Image != null)
            {
                if (user.ImageId != null)
                {
                    await this.usersService.DeleteUserImgAsync(user.UserName);
                }

                await this.usersService.AddUserImageAsync(this.Input.Image, user.UserName);
            }

            this.efRepository.Update(user);
            await this.efRepository.SaveChangesAsync();

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = ProfileUpdateMsg;
            return this.RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
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

            var userId = await this.userManager.GetUserIdAsync(user);
            var email = await this.userManager.GetEmailAsync(user);
            var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = this.Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: this.Request.Scheme);
            await this.emailSender.SendEmailAsync(
                email,
                EmailTemplateTitle,
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            this.StatusMessage = InfoMsgEmailSended;
            return this.RedirectToPage();
        }

        public class InputModel /*: IMapFrom<ApplicationUser>, IHaveCustomMappings*/
        {
            private const string DescriptionErrorMsg = "Your {0} cannot be with lower than {1} symbols";

            private const string NameErrorMsg = "Your {0} cannot be with more than {1} and lower than {2} symbols";

            // I cant put the string (jpg,jpeg,png,pdf) in the const dinamically because its const and asp.net core throws FormatException
            private const string ImgExtsErrorMsg = "Your {0} extension should be one of the following: jpg,jpeg,png,pdf";
            private const string NameRegexErrorMsg = "You cannot have more than one capital letter, not any other symbols except Latin alphabets";

            private const string CountryCodeRequredErrorMsg = "You are obligated to select your country";

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
            [ImageExtensions(GlobalConstants.AllowedImageExtensions, ErrorMessage = ImgExtsErrorMsg)]
            public ImageInputModel Image { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "email")]
            public string Email { get; set; }

            [Required(ErrorMessage = CountryCodeRequredErrorMsg)]
            public string Country { get; set; }

            public string ImageCloudUrl { get; set; }

            public string Profession { get; set; }
        }
    }
}
