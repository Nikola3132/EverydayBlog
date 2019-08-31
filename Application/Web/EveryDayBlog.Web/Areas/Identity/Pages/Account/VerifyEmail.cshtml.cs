namespace EveryDayBlog.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Data;
    using EveryDayBlog.Web.Infrastructure.Extensions;
    using EveryDayBlog.Web.Infrastructure.ModelBinders;
    using EveryDayBlog.Web.Infrastructure.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class VerifyEmailModel : PageModel
    {
        private const string ConfirmEmailMsg = "You've already confirmed your email.";
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService emailService;
        private readonly ILogger<VerifyEmailModel> logger;

        public VerifyEmailModel(
            UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            ILogger<VerifyEmailModel> logger)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.logger = logger;
        }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [ModelBinder(typeof(TempDataSerializedToObjectModelBinder<EmailViewModel>))]
        public EmailViewModel EmailViewModel { get; set; }

        public async Task<IActionResult> OnGet(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
            }

            if (this.User.Identity.IsAuthenticated)
            {
                this.TempData["alert"] = ConfirmEmailMsg;
                return this.Redirect("~/Home/Index");
            }

            returnUrl = returnUrl ?? this.Url.Content("~/");

            TempDataExtensions.Put<EmailViewModel>(this.TempData, "EmailOptions", new EmailViewModel { Email = this.EmailViewModel.Email, CallbackUrl = this.EmailViewModel.CallbackUrl });

            this.ReturnUrl = returnUrl;

            return this.Page();
        }

        public async Task<IActionResult> OnGetResentAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
            }

            if (this.User.Identity.IsAuthenticated)
            {
                this.TempData["alert"] = ConfirmEmailMsg;
                return this.Redirect("~/Home/Index");
            }

            returnUrl = returnUrl ?? this.Url.Content("~/");

            await this.emailService.SendEmailToUserAsync(this.EmailViewModel.CallbackUrl, this.EmailViewModel.Email);

            this.ReturnUrl = returnUrl;

            return this.Redirect("~/Identity/Account/VerifyEmail");
        }
    }
}
