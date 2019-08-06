namespace EveryDayBlog.Web.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using EveryDayBlog.Common;
    using EveryDayBlog.Data;
    using EveryDayBlog.Services.Data;
    using EveryDayBlog.Web.ViewModels.Home.ViewModels;
    using EveryDayBlog.Web.ViewModels.Posts.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using X.PagedList;

    public class HomeController : BaseController
    {
        private const int DefaultPageNumber = 1;
        private const int DefaultPageSize = 8;
        private const string NoResultsFound = "No results found!";



        private readonly IPostService postService;

        public HomeController(
            IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet]
        public IActionResult Index(IndexViewModel model)
        {
            var posts = this.postService.GetPostsFilter<IndexPostViewModel>(model.SearchString);
            posts = this.postService.OrderBy(posts, model.SortBy).ToList();

            int pageNumber = model.PageNumber ?? DefaultPageNumber;
            int pageSize = DefaultPageSize;

            var pageProductsViewModel = posts.ToPagedList(pageNumber, pageSize);

            model.ProductsViewModel = pageProductsViewModel;

            return this.View(model);
        }


        public IActionResult GetProduct(string term)
        {
            var products = this.postService.GetPostsBySearch<IndexPostViewModel>(term);

            if (products.Count() == 0)
            {
                return this.Json(new List<SearchViewModel>
                {
                    new SearchViewModel { Value = NoResultsFound, Url = string.Empty },
                });
            }

            var result = products.Select(x => new SearchViewModel
            {
                Value = x.PageHeader.Title,
                Url = string.Format(GlobalConstants.UrlTemplateAutoComplete, x.Id),
            });

            return Json(result);
        }

        [HttpGet]
        public IActionResult About()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}
