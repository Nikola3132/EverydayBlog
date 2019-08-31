namespace EveryDayBlog.Services.Data.Tests.ViewModels
{
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Mapping;

    public class PostViewTestModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public PageHeaderTestViewModel PageHeader { get; set; }
    }
}
