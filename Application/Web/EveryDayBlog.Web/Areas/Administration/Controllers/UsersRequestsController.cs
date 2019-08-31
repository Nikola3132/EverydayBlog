namespace EveryDayBlog.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using EveryDayBlog.Services.Data;
    using EveryDayBlog.Web.Areas.Administration.ViewModels.Messages.ViewModel;
    using EveryDayBlog.Web.ViewModels.Images.ViewModels;
    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using EveryDayBlog.Web.ViewModels.UsersRequests.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class UsersRequestsController : AdminBaseController
    {
        private const string Url = @"mailto:viewModel.Email?subject=Re: viewModel.Name&body=%0D%0A%0D%0ARe: viewModel.Message""";
        private const string NonExistingMsgIdErrorMsg = "There is no message with that Id.";
        private readonly IUserRequestService userRequestService;
        private readonly IPageHeaderService pageHeaderService;

        public UsersRequestsController(
            IUserRequestService userRequestService,
            IPageHeaderService pageHeaderService)
        {
            this.userRequestService = userRequestService;
            this.pageHeaderService = pageHeaderService;
        }

        [HttpGet]
        public async Task<IActionResult> Messages()
        {
            var unreadedMessages = await this.userRequestService.TakeAllNonReadedRequests<UserRequestDetailsViewModel>();
            var readedMessages = await this.userRequestService.TakeAllReadedRequests<UserRequestDetailsViewModel>();

            var viewModel = new MessagesViewModel
            {
                PageHeader = new PageHeaderViewModel { Image = new ImageBackgroundViewModel { } },
                UserRequestDetailsReaded = readedMessages,
                UserRequestDetailsUnReaded = unreadedMessages,
            };

            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Readed(int id)
        {
            var isReadedSucceded = await this.userRequestService.MarkAsReadedAsync(id);
            if (isReadedSucceded == false)
            {
                this.TempData["alert"] = NonExistingMsgIdErrorMsg;
            }

            return this.RedirectToAction("Messages");
        }

        [HttpGet]
        public async Task<ActionResult> SoftDelete(int id)
        {
            var isDeletedSuccedded = await this.userRequestService.SoftDeleteAsync(id);

            if (isDeletedSuccedded == false)
            {
                this.TempData["alert"] = NonExistingMsgIdErrorMsg;
            }

            return this.RedirectToAction("Messages");
        }

        [HttpGet]
        public async Task<ActionResult> Answer(int id)
        {
            var viewModel = await this.userRequestService.TakeUserRequestById<UserRequestDetailsViewModel>(id);
            if (viewModel == null)
            {
                this.TempData["alert"] = NonExistingMsgIdErrorMsg;
                return this.RedirectToAction("Messages");
            }

            return this.Redirect(Url);
        }

        [HttpGet]
        public async Task<IActionResult> Deleted(DeletedMessagesViewModel deletedMessagesViewModel = null)
        {
            if (deletedMessagesViewModel == null)
            {
                var deletedMessages = await this.userRequestService.TakeAllDeletedRequests<UserRequestDetailsViewModel>();

                deletedMessagesViewModel = new DeletedMessagesViewModel
                {
                    PageHeader = new PageHeaderViewModel { Image = new ImageBackgroundViewModel { } },
                    DeletedMessages = deletedMessages,
                };
            }
            else if (deletedMessagesViewModel.DeletedMessages == null || deletedMessagesViewModel.DeletedMessages.Count == 0)
            {
                if (await this.userRequestService.AnyDeletedUserRequests())
                {
                    var deletedMessages = await this.userRequestService.TakeAllDeletedRequests<UserRequestDetailsViewModel>();
                    deletedMessagesViewModel.DeletedMessages = deletedMessages;
                }
            }

            return this.View(deletedMessagesViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Deleted(int id)
        {
            await this.userRequestService.HardDeleteAsync(id);

            var deletedMessages = await this.userRequestService.TakeAllDeletedRequests<UserRequestDetailsViewModel>();
            var viewModel = new DeletedMessagesViewModel
            {
                PageHeader = new PageHeaderViewModel { Image = new ImageBackgroundViewModel { } },
                DeletedMessages = deletedMessages,
            };
            return this.View(viewModel);
        }
    }
}
