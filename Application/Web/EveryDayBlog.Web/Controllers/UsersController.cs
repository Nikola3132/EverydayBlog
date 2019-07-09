namespace EveryDayBlog.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        [HttpGet]
        public IActionResult Profile()
        {
            return this.View();
        }
    }
}