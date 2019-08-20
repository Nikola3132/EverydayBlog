namespace EveryDayBlog.Web.ViewModels.Images.ViewModels
{
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Mapping;

    public class ImageBackgroundViewModel : IMapFrom<Image>
    {
        public string CloudUrl { get; set; }
    }
}
