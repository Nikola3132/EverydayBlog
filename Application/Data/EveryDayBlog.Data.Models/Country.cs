namespace EveryDayBlog.Data.Models
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using EveryDayBlog.Data.Common.Models;

    public class Country
    {
        [Required]
        [MaxLength(56)]
        public string Name { get; set; }

        [Key]
        public string Code { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
        = new HashSet<ApplicationUser>();
    }
}
