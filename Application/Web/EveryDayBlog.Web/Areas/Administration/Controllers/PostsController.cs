namespace EveryDayBlog.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using EveryDayBlog.Services.Data;
    using EveryDayBlog.Web.Areas.Administration.ViewModels.Posts.InputModels;
    using EveryDayBlog.Web.Areas.Administration.ViewModels.Posts.ViewModels;
    using EveryDayBlog.Web.Infrastructure.Models;
    using EveryDayBlog.Web.ViewModels.PageHeaders.InputModels;
    using EveryDayBlog.Web.ViewModels.Sections.InputModels;
    using Microsoft.AspNetCore.Mvc;

    public class PostsController : AdministrationController
    {
        private readonly IPostService postService;
        private readonly IPageHeaderService pageHeaderService;

        public PostsController(
            IPostService postService,
            IPageHeaderService pageHeaderService)
        {
            this.postService = postService;
            this.pageHeaderService = pageHeaderService;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int postId)
        {
            var editPostViewModel = await this.postService.GetPostByIdAsync<EditPostViewModel>(postId);
            var img = editPostViewModel?.PageHeader?.Image;
            var editPostInputModel = new EditPostInputModel
            {
                PageHeader = new PageHeaderInputModel
                {
                    Image = new ImageInputModel
                    {
                        CloudUrl = img?.CloudUrl,
                    },
                    MainTitle = editPostViewModel?.PageHeader.Title,
                    SubTitle = editPostViewModel?.PageHeader.SubTitle,
                },
                Sections = editPostViewModel?.Sections.Where(s => s.SectionIsDeleted == false).Select(s => new EditSectionInputModel
                {
                    Id = s.SectionId,
                    SectionContent = s.SectionContent,
                    SectionTitle = s.SectionTitle,

                }).ToList(),
                Id = postId,
            };
            this.TempData["img"] = editPostInputModel?.PageHeader?.Image?.CloudUrl;

            return this.View(editPostInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int postId, EditPostInputModel editPostInputModel)
        {
            if (postId == 0)
            {
                postId = editPostInputModel.Id;
            }
            var editPostViewModel = await this.postService.GetPostByIdAsync<EditPostViewModel>(postId);
            if (!this.ModelState.IsValid)
            {
                this.TempData["img"] = editPostViewModel?.PageHeader?.Image?.CloudUrl;
                editPostInputModel = this.FillThePost(editPostViewModel, editPostInputModel);
                editPostInputModel.Id = postId;
                return this.View(editPostInputModel);
            }

            int pageHeaderId = await this.postService.GetPageHeaderIdAsync(postId);

            var img = editPostInputModel.PageHeader.Image;

            await this.pageHeaderService.UpdateAsync(pageHeaderId, new PageHeaderInputModel
            {
                Image = img,
                MainTitle = editPostInputModel.PageHeader.MainTitle,
                SubTitle = editPostInputModel.PageHeader.SubTitle,
            }
            );
            //UPDATE POST
            //var isPostCreated = await this.postService.CreatePostAsync(, this.User.Identity.Name);

            //if (!isPostCreated)
            //{
            //    this.TempData["alert"] = "Something went wrong!";
            //}

            return this.RedirectToAction("Edit", "Posts", new { postId});
        }

        [HttpGet]
        public async Task<IActionResult> Hide(int id)
        {
           bool isHide = await this.postService.HidePostByIdAsync(id);

           if (isHide == false)
            {
                this.TempData["alert"] = "Something went wrong!";
            }

           return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Hidden()
        {
            var hiddenPosts = await this.postService.AllHidenPosts<HiddenPostViewModel>();
            return this.View(hiddenPosts);
        }

        public async Task<IActionResult> Reorganize(int id)
        {
            var isReorganized = await this.postService.MakeVisibleAsync(id);
            if (isReorganized == false)
            {
                this.TempData["alert"] = "Something went wrong!";
            }

            return this.RedirectToAction("Hidden");
        }

        [NonAction]
        private EditPostInputModel FillThePost(EditPostViewModel editPostViewModel, EditPostInputModel editPostInputModel)
        {
            if (editPostInputModel.PageHeader?.Image?.CloudUrl == null)
            {
                editPostInputModel.PageHeader.Image = new ImageInputModel { CloudUrl = this.TempData["img"].ToString(), };
            }

            editPostInputModel.Sections = editPostViewModel?.Sections.Where(s => s.SectionIsDeleted == false).Select(s => new EditSectionInputModel
            {
                Id = s.SectionId,
                SectionContent = s.SectionContent,
                SectionTitle = s.SectionTitle,
            }).ToList();

            return editPostInputModel;
        }
    }
}
