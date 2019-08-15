namespace EveryDayBlog.Web.Areas.Administration.ViewModels.Home.ViewModels
{
    using System.Collections.Generic;

    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using EveryDayBlog.Web.ViewModels.Posts.ViewModels;

    public class IndexViewModel
    {
        public PageHeaderViewModel PageHeader { get; set; }

        public List<IndexPostViewModel> Posts { get; set; }
    }
}
