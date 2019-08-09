namespace EveryDayBlog.Web.ViewModels.Posts.ViewModels
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Mapping;
    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using EveryDayBlog.Web.ViewModels.Sections.InputModels;
    using EveryDayBlog.Web.ViewModels.Sections.ViewModels;
    using EveryDayBlog.Web.ViewModels.Users.ViewModels;

    public class IndexPostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public PageHeaderViewModel PageHeader { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<SectionViewModel> Sections { get; set; }
        = new List<SectionViewModel>();

        public SectionInputModel SectionInputModel { get; set; }

        public UserPreviewViewModel User { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, IndexPostViewModel>().ForMember(
                m => m.UserId,
                opt => opt.MapFrom(x => x.User.Id));

            configuration.CreateMap<Post, IndexPostViewModel>().ForMember(
                m => m.Sections,
                opt => opt.MapFrom(x => x.PostSections));

        }
    }
}
