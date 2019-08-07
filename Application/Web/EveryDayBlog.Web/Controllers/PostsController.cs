namespace EveryDayBlog.Web.Controllers
{
    using EveryDayBlog.Services.Data;
    using EveryDayBlog.Web.ViewModels.Posts.InputModels;
    using EveryDayBlog.Web.ViewModels.Posts.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    [Authorize]
    public class PostsController : BaseController
    {
        private readonly IPostService postService;
        private readonly ILogger<PostsController> logger;

        public PostsController(
            IPostService postService,
            ILogger<PostsController> logger)
        {
            this.postService = postService;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Create() => this.View();

        [HttpPost]
        public async Task<IActionResult> Create(PostInputModel post)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(post);
            }

            var isPostCreated = await this.postService.CreatePostAsync(post, this.User.Identity.Name);

            if (!isPostCreated)
            {
                this.TempData["alert"] = "Something went wrong! We'll look at the problem and soon if all is well, your post will be uploaded!";
                this.logger.LogError("Post cannot be saved in the database.");

                // TODO: Manually upload the post from admin!
            }

            return this.Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var post = await this.postService.GetPostByIdAsync<IndexPostViewModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            return this.View(post);
        }
    }
}
