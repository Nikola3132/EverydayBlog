namespace EveryDayBlog.Web.Areas.Administration.ViewModels.Sections.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Mapping;

    public class EditSectionViewModel : IMapFrom<Section>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
