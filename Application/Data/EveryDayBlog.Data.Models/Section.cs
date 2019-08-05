namespace EveryDayBlog.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using EveryDayBlog.Data.Common.Models;

    public class Section : BaseDeletableModel<int>, IPostFormable
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}