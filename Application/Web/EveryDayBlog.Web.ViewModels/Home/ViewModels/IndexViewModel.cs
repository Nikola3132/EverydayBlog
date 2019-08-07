namespace EveryDayBlog.Web.ViewModels.Home.ViewModels
{
    using AutoMapper;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Data.Models.Enums;
    using EveryDayBlog.Services.Mapping;
    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using EveryDayBlog.Web.ViewModels.Posts.ViewModels;
    using EveryDayBlog.Web.ViewModels.Users.ViewModels;
    using X.PagedList;

    public class IndexViewModel 
    {
        public IPagedList<IndexPostViewModel> PostsViewModel { get; set; }

        public PageHeaderViewModel PageHeader { get; set; }

        public string SearchString { get; set; }

        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public PostsSort SortBy { get; set; }


        
    }
}
