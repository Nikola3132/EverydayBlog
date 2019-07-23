namespace EveryDayBlog.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EveryDayBlog.Data.Common.Repositories;
    using EveryDayBlog.Data.Models;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        private readonly IDeletableEntityRepository<ApplicationUser> users;

        public UsersController(IDeletableEntityRepository<ApplicationUser> users)
        {
            this.users = users;
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return this.View();
        }
    }
}