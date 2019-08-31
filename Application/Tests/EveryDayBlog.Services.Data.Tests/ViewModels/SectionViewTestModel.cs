namespace EveryDayBlog.Services.Data.Tests.ViewModels
{
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Mapping;

    public class SectionViewTestModel : IMapFrom<Section>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        public bool IsDeleted { get; set; }
    }
}
