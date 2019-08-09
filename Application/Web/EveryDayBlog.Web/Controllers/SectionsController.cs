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
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Authorize]
    public class SectionsController : BaseController
    {
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
                this.TempData["alert"] = "Your section wasn't added ,because of input errors!";
                return this.RedirectToAction("Details", "Posts", new { id = id });

            }


            var sectionId = await this.sectionService.CreateSectionAsync(sectionInputModel, id);

            if (sectionId == null)
            {
                this.TempData["alert"] = "Something went wrong! We'll look at the problem and soon if all is well, your section will be added!";
                this.logger.LogError($"The section cannot be saved in the database <-----> postId -> {id}.");

                // TODO: Manually upload the post from admin!
            }

            return this.RedirectToAction("Details", "Posts", new { id= id });
        }
        // GET: Sections
        public ActionResult Index()
        {
            return View();
        }

        // GET: Sections/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Sections/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Sections/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Sections/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sections/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}