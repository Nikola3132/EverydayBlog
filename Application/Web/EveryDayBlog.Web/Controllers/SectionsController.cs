namespace EveryDayBlog.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Data;
    using EveryDayBlog.Web.ViewModels.Posts.ViewModels;
    using EveryDayBlog.Web.ViewModels.Sections.InputModels;
    using EveryDayBlog.Web.ViewModels.Sections.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Authorize]
    public class SectionsController : BaseController
    {
        private const string SectionErrorMsg = "Your section wasn't added ,because of input errors!";
        private const string SectionAddErrorMsg = "Something went wrong! We'll look at the problem and soon if all is well, your section will be added!";

        private readonly ISectionService sectionService;
        private readonly ILogger<SectionsController> logger;

        public SectionsController(
            ISectionService sectionService,
            ILogger<SectionsController> logger)
        {
            this.sectionService = sectionService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Add(SectionInputModel sectionInputModel)
        {
            var id = int.Parse(this.TempData["ProductId"].ToString());

            if (!this.ModelState.IsValid)
            {
                this.TempData["alert"] = SectionErrorMsg;
                return this.RedirectToAction("Details", "Posts", new { id = id });
            }

            var sectionId = await this.sectionService.CreateSectionAsync(sectionInputModel, id);

            if (sectionId != 0)
            {
                return this.RedirectToAction("Details", "Posts", new { id = id });
            }

            this.TempData["alert"] = SectionAddErrorMsg;
            this.logger.LogError(message: $"The section cannot be saved in the database <-----> postId -> {id}.");

            return this.RedirectToAction("Details", "Posts", new { id = id });
        }

        // GET: Sections
        public ActionResult Index()
        {
            return this.View();
        }

        // GET: Sections/Details/5
        public ActionResult Details(int id)
        {
            return this.View();
        }

        // GET: Sections/Create
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: Sections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return this.RedirectToAction(nameof(this.Index));
            }
            catch
            {
                return this.View();
            }
        }

        public ActionResult Delete(int id)
        {
            return this.View();
        }

        // POST: Sections/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return this.RedirectToAction(nameof(this.Index));
            }
            catch
            {
                return this.View();
            }
        }
    }
}
