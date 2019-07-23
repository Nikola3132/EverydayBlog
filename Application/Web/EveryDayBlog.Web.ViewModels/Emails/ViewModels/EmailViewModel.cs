namespace EveryDayBlog.Web.ViewModels.Emails.ViewModels
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.ComponentModel.DataAnnotations;

    public class EmailViewModel
    {
        public string CallbackUrl { get; set; }

        public string Email { get; set; }
    }
}
