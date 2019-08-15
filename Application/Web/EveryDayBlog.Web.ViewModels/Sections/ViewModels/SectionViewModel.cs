using EveryDayBlog.Data.Models;
using EveryDayBlog.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayBlog.Web.ViewModels.Sections.ViewModels
{
    public class SectionViewModel : IMapFrom<SectionPost>
    {
        public int SectionId { get; set; }

        public string SectionTitle { get; set; }

        public string SectionContent { get; set; }

        public bool SectionIsDeleted { get; set; }
    }
}
