namespace EveryDayBlog.Web.Areas.Administration.ViewModels.Posts.ViewModels
{
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Mapping;
    using EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class HiddenPostViewModel : IMapFrom<Post>
    {
        [BindNever]
        public int Id { get; set; }

        public PageHeaderViewModel PageHeader { get; set; }
    }
}
