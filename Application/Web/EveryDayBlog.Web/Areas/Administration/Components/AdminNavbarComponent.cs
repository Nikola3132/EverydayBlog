namespace EveryDayBlog.Web.Areas.Administration.Components
{
    using EveryDayBlog.Services.Data;
    using EveryDayBlog.Web.ViewModels.AdminNavbars.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class AdminNavbarComponent : ViewComponent
    {
        private readonly IUserRequestService userRequestService;

        public AdminNavbarComponent(IUserRequestService userRequestService)
        {
            this.userRequestService = userRequestService;
        }

        public IViewComponentResult Invoke()
        {
            var readedRequests = this.userRequestService.TakeAllReadedRequestsCount();
            var unreadedRequests = this.userRequestService.TakeAllUnReadedRequestsCount();
            var allRequests = this.userRequestService.TakeAllRequestsCount();
            var viewModel = new AdminNavbarViewModel
            {
                UnReadedRequests = unreadedRequests,
                AllRequestsCount = allRequests,
                ReadedRequests = readedRequests,
            };

            return this.View(viewModel);
        }
    }
}
