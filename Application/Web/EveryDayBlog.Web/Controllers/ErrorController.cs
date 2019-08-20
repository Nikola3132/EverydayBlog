namespace EveryDayBlog.Web.Controllers
{
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : BaseController
    {
        public IActionResult Error500()
        {
            var exceptionFeature = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                this.ViewBag.ErrorMessage = exceptionFeature.Error.Message;
            }

            return this.View();
        }
    }
}
