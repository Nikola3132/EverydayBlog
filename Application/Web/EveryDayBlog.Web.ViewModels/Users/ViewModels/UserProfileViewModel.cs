using EveryDayBlog.Data.Models;
using EveryDayBlog.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayBlog.Web.ViewModels.Users.ViewModels
{
    public class UserProfileViewModel : IMapTo<ApplicationUser>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Description { get; set; }

        public Image Image { get; set; }

        public string Email { get; set; }
    }

}
