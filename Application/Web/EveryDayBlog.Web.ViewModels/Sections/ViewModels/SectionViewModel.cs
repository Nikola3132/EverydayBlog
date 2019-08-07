using EveryDayBlog.Data.Models;
using EveryDayBlog.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayBlog.Web.ViewModels.Sections.ViewModels
{
    public class SectionViewModel: IMapFrom<Section>
    {
        public string Title { get; set; }

        public string Content { get; set; }
    }
}
