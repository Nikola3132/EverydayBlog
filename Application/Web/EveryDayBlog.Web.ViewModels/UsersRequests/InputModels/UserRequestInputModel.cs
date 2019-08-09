namespace EveryDayBlog.Web.ViewModels.UsersRequests.InputModels
{
    using EveryDayBlog.Web.ViewModels.PageHeaders.InputModels;
    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using System.ComponentModel.DataAnnotations;

    public class UserRequestInputModel
    {

        [BindProperty]
        public PageHeaderViewModel PageHeader { get; set; }
        = new PageHeaderViewModel();

        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        [Display(Name = "Full name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}
