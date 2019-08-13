namespace EveryDayBlog.Web.Areas.Administration.ViewModels.Home.ViewModels
{
    using System.Collections.Generic;

    using EveryDayBlog.Web.Areas.Administration.ViewModels.Posts.ViewModels;
    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;

    public class IndexViewModel
    {
        public PageHeaderViewModel PageHeader { get; set; }

        public List<PostViewModel> Posts { get; set; }
    }
}
