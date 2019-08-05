using EveryDayBlog.Web.Infrastructure.Models;

namespace EveryDayBlog.Services
{
    public interface ICloudinaryService
    {
       string UploudPicture(ImageInputModel image, string folderName);
    }
}
