namespace EveryDayBlog.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using EveryDayBlog.Data.Common.Models;
    public class UserRequest : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]
        public string Phone { get; set; }

        [Required]
        public string Message { get; set; }

        public bool IsReaded { get; set; } = false;
    }
}
