namespace EveryDayBlog.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using EveryDayBlog.Data.Common.Models;

    public class Image : BaseDeletableModel<int>
    {
        public string ImageTitle { get; set; }

        public string CloudUrl { get; set; }

        [NotMapped]
        [Required]
        public byte[] ImageByte { get; set; }

        public string ImagePath { get; set; }

        [Required]
        public string ContentType { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
        = new HashSet<ApplicationUser>();
    }
}
