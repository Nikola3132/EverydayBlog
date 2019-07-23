namespace EveryDayBlog.Services.Extensions
{
    public class ImageNameExtensions : IImageNameExtensions
    {
        private const string ImgTitlePrefix = "profile ";

        public string CreateImageTitle(string fileName)
        {
            return ImgTitlePrefix + fileName;
        }

        public string TakeUserIdFromImgTitle(string imgTitle)
        {
            return imgTitle.Substring(8, imgTitle.Length - 8);
        }
    }
}
