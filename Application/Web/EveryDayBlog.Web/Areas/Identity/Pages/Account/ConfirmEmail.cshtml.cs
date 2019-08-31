namespace EveryDayBlog.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Data.Repositories;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Caching.Distributed;

    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private const string ConfirmingEmailMsg = "Thanks you for confirming your email!";
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IDistributedCache distributedCache;

        public ConfirmEmailModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IDistributedCache distributedCache)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.distributedCache = distributedCache;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null)
            {
                return this.RedirectToPage("/Index");
            }

            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return this.NotFound(value: $"Unable to load user with ID '{userId}'.");
            }

            var changeToNewEmail = await this.distributedCache.GetStringAsync(user.Email);
            await this.distributedCache.RemoveAsync(user.Email);

            if (changeToNewEmail != null)
            {
                var setEmailResult = await this.userManager.SetEmailAsync(user, changeToNewEmail);

                if (!setEmailResult.Succeeded)
                {
                    var currentUserId = await this.userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException(message: $"Unexpected error occurred setting email for user with ID '{currentUserId}'.");
                }

                await this.userManager.SetUserNameAsync(user, changeToNewEmail);
                code = await this.signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
            }

            var result = await this.userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(message: $"Error confirming email for user with ID '{userId}':");
            }

            this.TempData["info"] = ConfirmingEmailMsg;

            await this.signInManager.SignInAsync(user, isPersistent: true);

            return this.RedirectToAction("Index", "Home", new { Area = string.Empty });
        }
    }
}
