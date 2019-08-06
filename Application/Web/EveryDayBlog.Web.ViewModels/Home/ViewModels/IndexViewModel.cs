namespace EveryDayBlog.Web.ViewModels.Home.ViewModels
{
    using EveryDayBlog.Data.Models.Enums;
    using EveryDayBlog.Web.ViewModels.Posts.ViewModels;
    using X.PagedList;

    public class IndexViewModel
    {
        public IPagedList<IndexPostViewModel> ProductsViewModel { get; set; }

        public string SearchString { get; set; }

        public int? PageNumber { get; set; }

        public PostsSort SortBy { get; set; }

    }
}
