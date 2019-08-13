namespace EveryDayBlog.Web.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using EveryDayBlog.Common;
    using EveryDayBlog.Data;
    using EveryDayBlog.Services.Data;
    using EveryDayBlog.Web.ViewModels.Home.ViewModels;
    using EveryDayBlog.Web.ViewModels.Posts.ViewModels;
    using EveryDayBlog.Web.ViewModels.UsersRequests.InputModels;
    using EveryDayBlog.Web.ViewModels.UsersRequests.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using X.PagedList;

    public class HomeController : BaseController
    {
        private const int DefaultPageNumber = 1;
        private const int DefaultPageSize = 5;
        private const string NoResultsFound = "No results found!";



        private readonly IPostService postService;
        private readonly IUserRequestService userRequestService;
        private readonly IUsersService usersService;



        public HomeController(
            IPostService postService,
            IUserRequestService userRequestService,
            IUsersService usersService)
        {
            this.postService = postService;
            this.userRequestService = userRequestService;
            this.usersService = usersService;


        }

        public async Task<IActionResult> Index(IndexViewModel model)
        {
            var posts = this.postService.GetPostsFilter<IndexPostViewModel>(model.SearchString);

            var sort = model.SortBy.ToString();
            if (sort == "Yours")
            {
                posts = await this.postService.OrderByAsync(posts, model.SortBy, this.User.Identity.Name);
            }
            else
            {
                posts = await this.postService.OrderByAsync(posts, model.SortBy);
            }

            int pageNumber = model.PageNumber ?? DefaultPageNumber;
            int pageSize = model.PageSize ?? DefaultPageSize;

            var pageProductsViewModel = posts.ToPagedList(pageNumber, pageSize);

            model.PostsViewModel = pageProductsViewModel;

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
        public async Task<IActionResult> Contact()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var currentUserFullName = await this.usersService.GetUserFullName(this.User.Identity.Name);
                var userForContact = new UserRequestInputModel();

                userForContact.Email = this.User.Identity.Name;

                if (!string.IsNullOrEmpty(currentUserFullName))
                {
                    userForContact.Name = currentUserFullName;
                }

                return this.View(userForContact);
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(UserRequestInputModel userRequestInputModel)
        {
            if (userRequestInputModel.Email == null)
            {
                userRequestInputModel.Email = this.User.Identity.Name;
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(userRequestInputModel);
            }

            var isQuestionSendedCreated = await this.userRequestService.SendQuestionAsync(userRequestInputModel);

            if (isQuestionSendedCreated)
            {
                this.TempData["info"] = "Your question was sent successfully! We'll answer you soon on the email you gave us!";
            }

            return this.Redirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}
