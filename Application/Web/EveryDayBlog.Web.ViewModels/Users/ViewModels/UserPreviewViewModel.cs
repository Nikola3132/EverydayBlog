using AutoMapper;
using EveryDayBlog.Data.Models;
using EveryDayBlog.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayBlog.Web.ViewModels.Users.ViewModels
{
    public class UserPreviewViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Description { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserPreviewViewModel>()
                .ForMember(
                    u => u.Country,
                    opt
                      => opt.MapFrom(x => x.Country.Name));
        }
    }
}