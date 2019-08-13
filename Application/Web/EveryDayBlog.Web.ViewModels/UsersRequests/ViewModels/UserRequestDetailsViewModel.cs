using EveryDayBlog.Data.Models;
using EveryDayBlog.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayBlog.Web.ViewModels.UsersRequests.ViewModels
{
    public class UserRequestDetailsViewModel : IMapFrom<UserRequest>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public string Phone { get; set; }

        public bool IsReaded { get; set; }
    }
}
