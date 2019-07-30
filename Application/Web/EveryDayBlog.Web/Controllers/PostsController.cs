namespace EveryDayBlog.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EveryDayBlog.Web.ViewModels.Posts.InputModels;
    using EveryDayBlog.Web.ViewModels.Sections.InputModels;
    using Microsoft.AspNetCore.Mvc;

    public class PostsController : BaseController
    {
        [HttpGet]
        public IActionResult Create()
        { 
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(PostInputModel post)
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return this.View();
        }
    }
}
