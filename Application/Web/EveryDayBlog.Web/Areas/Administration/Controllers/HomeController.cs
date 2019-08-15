namespace EveryDayBlog.Web.Areas.Administration.Controllers
{
    using System.Linq;

    using EveryDayBlog.Services.Data;
    using EveryDayBlog.Web.Areas.Administration.ViewModels.Home.ViewModels;
    using EveryDayBlog.Web.ViewModels.Images.ViewModels;
    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using EveryDayBlog.Web.ViewModels.Posts.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : AdministrationController
    {

        private readonly IPostService postService;

        public HomeController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var allNotHidenPosts = this.postService.GetVisiblePosts<IndexPostViewModel>().ToList();

            var viewModel = new IndexViewModel()
            {
                PageHeader = new PageHeaderViewModel { Image = new ImageBackgroundViewModel { } },
                Posts = allNotHidenPosts,
            };
            return this.View(viewModel);
        }
    }
}
