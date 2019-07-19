namespace EveryDayBlog.Web.Controllers
{
    using System.IO;
    using System.Linq;

    using EveryDayBlog.Data;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
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

        [HttpGet]
        public FileStreamResult ImageTest()
        {
            var image = db.Images.FirstOrDefault();

            MemoryStream ms = new MemoryStream(image.ImageByte);
            return new FileStreamResult(ms, image.ContentType);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}
