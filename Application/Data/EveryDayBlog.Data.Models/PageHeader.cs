namespace EveryDayBlog.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using EveryDayBlog.Data.Common.Models;

    public class PageHeader : BaseDeletableModel<int>, IPostFormable
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string SubTitle { get; set; }

        [Required]
        public string PageIndicator { get; set; }

        [ForeignKey("Image")]
        public int? ImageId { get; set; }

        public Image Image { get; set; }
    }
}
