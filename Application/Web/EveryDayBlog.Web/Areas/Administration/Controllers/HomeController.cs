namespace EveryDayBlog.Web.Areas.Administration.Controllers
{
    using EveryDayBlog.Services.Data;
    using EveryDayBlog.Web.Areas.Administration.ViewModels.Dashboard;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : AdministrationController
    {

        public HomeController()
        {

        }

        public IActionResult Index()
        {
            var viewModel = new EveryDayBlog.Web.Areas.Administration.ViewModels.Home.ViewModels.IndexViewModel()
            {
                PageHeader = new Web.ViewModels.PageHeaders.ViewModels.PageHeaderViewModel
                {
                    Image = new Web.ViewModels.Images.ViewModels.ImageBackgroundViewModel
                    {
                        CloudUrl = "https://res.cloudinary.com/dy78wnfy2/image/upload/v1565342409/PageHeaders/589c5691-5d07-4421-b5a1-fcc1179b0616.jpg"
                    }
                ,
                    Title = "fsfsd",
                    SubTitle = " sdfsdfsdf"
                }
            };
            return this.View(viewModel);
        }
    }
}
