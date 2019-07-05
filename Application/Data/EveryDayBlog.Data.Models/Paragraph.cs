namespace EveryDayBlog.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using EveryDayBlog.Data.Common.Models;

    public class Paragraph : BaseDeletableModel<int>
    {
        [Required]
        public string MainText { get; set; }

        [Required]
        [ForeignKey("Image")]
        public int ImageId { get; set; }

        public Image Image { get; set; }
    }
}