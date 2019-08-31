namespace EveryDayBlog.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Common.Repositories;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services;
    using EveryDayBlog.Services.Data;
    using EveryDayBlog.Services.Mapping;
    using EveryDayBlog.Web.ViewModels.Settings;
    using EveryDayBlog.Web.ViewModels.Settings.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class SettingsController : BaseController
    {
        private readonly IDeletableEntityRepository<Setting> repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IUsersService userService;

        public SettingsController(
            IDeletableEntityRepository<Setting> repository,
            UserManager<ApplicationUser> userManager,
            ICloudinaryService cloudinaryService,
            IUsersService userService)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.cloudinaryService = cloudinaryService;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var settings = this.repository.All().To<SettingViewModel>().ToList();
            var model = new SettingsListViewModel { Settings = settings };
            return this.View(model);
        }

        public async Task<IActionResult> DeleteImg()
        {
           //var userImageUrl = this.userService.GetUserProfileImageIfExists(this.User.Identity.Name);

           // this.cloudinaryService.DeleteAnImage(userImageUrl);

            var settings = this.repository.All().To<SettingViewModel>().ToList();
            var model = new SettingsListViewModel { Settings = settings };

            return this.Content("Ready");
        }

        public async Task<IActionResult> InsertSetting()
        {
            var random = new Random();
            var setting = new Setting { Name = $"Name_{random.Next()}", Value = $"Value_{random.Next()}" };

            await this.repository.AddAsync(setting);
            await this.repository.SaveChangesAsync();

            return this.RedirectToAction(nameof(this.Index));
        }
        public IActionResult ChangeEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangeEmail(string email)
        {
            //db Entities updateaction = new dbEntities();
            //int id = (Convert.ToInt32(Session["id"]));

            //string myinfo = info;
            //Product pp = updateaction.Product.Where(m => m.database_id.Equals(id) && m.name.Equals(myinfo)).SingleOrDefault();
            //pp.price = price;
            //pp.product = product;
            //int i = updateaction.SaveChanges();
            //Session["warning"] = i;
            return Json(new { success = true, responseText = " Sucessfully." });
        }

    }
}
