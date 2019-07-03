namespace EveryDayBlog.Web.Areas.Administration.Controllers
{
    using EveryDayBlog.Common;
    using EveryDayBlog.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
