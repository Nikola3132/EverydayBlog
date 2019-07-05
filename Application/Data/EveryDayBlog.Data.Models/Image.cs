namespace EveryDayBlog.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using EveryDayBlog.Data.Common.Models;

    public class Image : BaseDeletableModel<int>
    {
        public string ImageTitle { get; set; }

        [Required]
        public byte[] ImageByte { get; set; }

        public string ImagePath { get; set; }
    }
}
