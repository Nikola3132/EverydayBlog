namespace EveryDayBlog.Services.Data.Tests.ViewModels
{
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Mapping;

    public class PageHeaderViewModel : IMapFrom<PageHeader>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string PageIndicator { get; set; }
    }
}
