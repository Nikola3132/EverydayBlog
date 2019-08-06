using AutoMapper;
using EveryDayBlog.Data.Models;
using EveryDayBlog.Services.Mapping;
using EveryDayBlog.Web.ViewModels.Images.ViewModels;
using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
using EveryDayBlog.Web.ViewModels.Sections.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayBlog.Web.ViewModels.Posts.ViewModels
{
    public class IndexPostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public PageHeaderViewModel PageHeader { get; set; }

        public string PostedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public SectionViewModel Section { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, IndexPostViewModel>().ForMember(
                m => m.PostedBy,
                opt => opt.MapFrom(x => x.User.FirstName + " " + x.User.LastName));
        }
    }
}
