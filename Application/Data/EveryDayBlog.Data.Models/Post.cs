namespace EveryDayBlog.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using EveryDayBlog.Data.Common.Models;

    public class Post : BaseDeletableModel<int>
    {
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public virtual ICollection<Section> Sections { get; set; }
        = new List<Section>();

        [Required]
        [ForeignKey("PageHeader")]
        public int PageHeaderId { get; set; }

        public PageHeader PageHeader { get; set; }
    }
}
