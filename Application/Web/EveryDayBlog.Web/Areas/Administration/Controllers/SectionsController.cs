namespace EveryDayBlog.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Data;
    using EveryDayBlog.Web.Areas.Administration.ViewModels.Sections.ViewModels;
    using EveryDayBlog.Web.ViewModels.Sections.InputModels;
    using EveryDayBlog.Web.ViewModels.Sections.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;

    public class SectionsController : AdministrationController
    {
        private const string Action = "Details";
        private const string Controller = "Posts";
        private const string EmailTemplateTitle = "One of your sections was deleted by the admin!";
        private const string ErrorMsg = "There was an error!";
        private readonly ISectionService sectionService;
        private readonly IPostService postService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;
        private readonly IUsersService usersService;

        public SectionsController(
           ISectionService sectionService,
           IPostService postService,
           UserManager<ApplicationUser> userManager,
           IEmailSender emailSender,
           IUsersService usersService)
        {
            this.sectionService = sectionService;
            this.postService = postService;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.usersService = usersService;
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, int postId, [Bind(Prefix = "sectionn")] EditSectionInputModel sectionInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Edit", "Posts", new { postId });
            }

            try
            {
                // TODO: Add update logic here
                var beforeUpdateSc = await this.sectionService.GetSectionByIdAsync<EditSectionViewModel>(id);
                if (beforeUpdateSc.Content != sectionInputModel.SectionContent || beforeUpdateSc.Title != sectionInputModel.SectionTitle)
                {
                    // TODO: Do magic if the boolean is false
                    await this.sectionService.UpdateSectionByIdAsync(id, sectionInputModel);
                }

                return this.RedirectToAction("Edit", "Posts", new { postId });
            }
            catch
            {
                return this.RedirectToAction("Edit", "Posts", new { postId });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id, int postId)
        {
            try
            {
                // TODO: Add update logic here
                var isDeleted = await this.sectionService.SoftDelete(id);
                if (!isDeleted)
                {
                    // TODO: Do some magic!
                }

                var userId = await this.postService.GetCreatorsIdAsync(postId);

                var user = await this.usersService.GetUserByIdAsync(userId);

                var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = this.Url.Action(
                      Action,
                      Controller,
                      new { id = postId },
                      protocol: this.Request.Scheme);

                await this.emailSender.SendEmailAsync(
                    user.Email,
                    EmailTemplateTitle,
                    $"Please click <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>here</a> and see in which of your posts was it. If you want clarification you can send email in the contact form with this number written like (sectionId => {id})");

                return this.RedirectToAction("Edit", "Posts", new { postId });
            }
            catch
            {
                this.TempData["alert"] = ErrorMsg;
                return this.RedirectToAction("Edit", "Posts", new { postId });
            }
        }

        [HttpGet]
        public async Task<ActionResult> Deleted()
        {
           var deletedSections = await this.sectionService.AllDeletedSections<DeletedSectionViewModel>();
           return this.View(deletedSections);
        }

        [HttpGet]
        public async Task<ActionResult> Reorganize(int id)
        {
            var sections = await this.sectionService.AllDeletedSections<ReorganizeSectionViewModel>();


            return this.View(sections.SingleOrDefault(section => section.Id == id));
        }

        [HttpPost]
        public async Task<ActionResult> Reorganize(ReorganizeSectionViewModel reorganizeSectionViewModel)
        {
            await this.sectionService.ReorganizeAsync(reorganizeSectionViewModel.Id);

            return this.RedirectToAction("Deleted");
        }
    }
}
