namespace EveryDayBlog.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SectionPost
    {
        public Post Post { get; set; }

        [Required]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        [Required]
        [ForeignKey("Section")]
        public int SectionId { get; set; }

        public Section Section { get; set; }

    }
}
