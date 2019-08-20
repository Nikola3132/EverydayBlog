namespace EveryDayBlog.Web.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class EmailViewModel
    {
        public string CallbackUrl { get; set; }

        public string Email { get; set; }
    }
}
