namespace EveryDayBlog.Web.Areas.Administration.ViewModels.Messages.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using EveryDayBlog.Web.ViewModels.UsersRequests.ViewModels;

    public class DeletedMessagesViewModel
    {
        public PageHeaderViewModel PageHeader { get; set; }
        = new PageHeaderViewModel();

        public List<UserRequestDetailsViewModel> DeletedMessages { get; set; }
        = new List<UserRequestDetailsViewModel>();
    }
}
