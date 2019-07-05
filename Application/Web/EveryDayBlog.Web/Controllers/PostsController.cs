using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayBlog.Web.Controllers
{
    public class PostsController : BaseController
    {
        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }
    }
}
