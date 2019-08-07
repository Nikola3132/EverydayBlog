namespace EveryDayBlog.Web.Areas.Identity.Pages.Account
{
    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [AllowAnonymous]
    public class ForgotPasswordConfirmation : PageModel
    {
        public PageHeaderViewModel PageHeader { get; set; }

        public void OnGet()
        {
        }
    }
}
