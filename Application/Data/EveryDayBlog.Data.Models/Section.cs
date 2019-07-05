namespace EveryDayBlog.Data.Models
{
    using System.Collections.Generic;

    using EveryDayBlog.Data.Common.Models;

    public class Section : BaseDeletableModel<int>, IPostFormable
    {
        public string Title { get; set; }

        public virtual ICollection<Paragraph> Paragraphs { get; set; }
    }
}