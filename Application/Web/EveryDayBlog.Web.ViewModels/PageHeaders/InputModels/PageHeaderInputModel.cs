namespace EveryDayBlog.Web.ViewModels.PageHeaders.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using EveryDayBlog.Common;
    using EveryDayBlog.Web.Infrastructure.CustomAttributes;
    using EveryDayBlog.Web.Infrastructure.ModelBinders;
    using EveryDayBlog.Web.Infrastructure.Models;
    using Microsoft.AspNetCore.Mvc;

    public class PageHeaderInputModel
    {
        private const string SubTitleErrorMsg = "Your {0} cannot be with more than {1} and lower than {2} symbols";
        private const string MainTitleErrorMsg = "Your {0} cannot be with more than {1} and lower than {2} symbols";

        // I cant put the string (jpg,jpeg,png,pdf) in the const dinamically because its const and asp.net core throws FormatException
        private const string ImgExtsErrorMsg = "Your {0} extension should be one of the following: jpg,jpeg,png,pdf";

        [Required]
        [StringLength(maximumLength: 40, MinimumLength = 3, ErrorMessage = MainTitleErrorMsg)]
        [DataType(DataType.Text)]
        [Display(Name = "title")]
        public string MainTitle { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = SubTitleErrorMsg)]
        [DataType(DataType.Text)]
        [Display(Name = "sub-title")]
        public string SubTitle { get; set; }

        [ModelBinder(typeof(FileToImageModelBinder))]
        [DataType(DataType.Upload)]
        [Display(Name = "image")]
        [ImageExtensions(GlobalConstants.AllowedImageExtensions, ErrorMessage = ImgExtsErrorMsg)]
        public ImageInputModel Image { get; set; }
    }
}
