namespace EveryDayBlog.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class PostsController : BaseController
    {
        [HttpGet]
        public IActionResult Create()
        { 
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(string myFormData)
        {
            return this.View();
        }
    }
}
