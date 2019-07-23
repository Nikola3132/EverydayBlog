namespace EveryDayBlog.Services.Extensions
{
    public interface IImageNameExtensions
    {
        string CreateImageTitle(string userId);

        string TakeUserIdFromImgTitle(string imgTitle);

    }
}
