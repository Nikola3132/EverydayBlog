namespace EveryDayBlog.Web.Areas.Identity.Pages.Account
{
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
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [AllowAnonymous]
    public class VerifyEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService emailService;
        private readonly ILogger<VerifyEmailModel> logger;

        //private readonly SendGridEmailSender sendGridEmailSender;

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
                this.TempData["alert"] = "You've already confirmed your email. We'll redirect you to Home Page!";

                Thread.Sleep(new TimeSpan(0, 0, 2));
                return this.Partial("~/Views/Shared/Partials/_ClosePartial.cshtml");
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
                Thread.Sleep(new TimeSpan(0, 0, 2));
                this.Partial("~/Views/Shared/Partials/_ClosePartial.cshtml");
            }

            returnUrl = returnUrl ?? this.Url.Content("~/");

            await this.emailService.SendEmailToUserAsync(this.EmailViewModel.CallbackUrl, this.EmailViewModel.Email);

            this.ReturnUrl = returnUrl;

            return this.Redirect("~/Identity/Account/VerifyEmail");
        }
    }
}