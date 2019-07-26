namespace EveryDayBlog.Web.ViewModels.Images.InputModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ImageInputModel
    {
        public string ImageTitle { get; set; }

        public string CloudUrl { get; set; }

        [Required]
        public byte[] ImageByte { get; set; }

        public string ImagePath { get; set; }

        [Required]
        public string ContentType { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
