using EveryDayBlog.Web.ViewModels.Sections.InputModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayBlog.Web.ViewModels.Posts.InputModels
{
    public class PostInputModel
    {
        public string MainTitle { get; set; }

        public string SubTitle { get; set; }

        public SectionInputModel Section { get; set; }
    }
}
