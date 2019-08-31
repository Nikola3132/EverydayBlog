namespace EveryDayBlog.Web.Areas.Administration.ViewModels.Posts.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Mapping;
    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using EveryDayBlog.Web.ViewModels.Sections.ViewModels;

    public class EditPostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public PageHeaderViewModel PageHeader { get; set; }

        public List<SectionViewModel> Sections { get; set; }
        = new List<SectionViewModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, EditPostViewModel>().ForMember(
                m => m.Sections,
                opt => opt.MapFrom(x => x.PostSections));
        }
    }
}
