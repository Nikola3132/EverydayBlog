namespace EveryDayBlog.Services.Data
{
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Web.ViewModels.Images.InputModels;

    public interface IUsersService
    {
        TEntity GetUserByUsername<TEntity>(string username);

        string GetUserImageIfExists(string username);

        Task<bool> AddUserImage(ImageInputModel imageInputModel, string username);

        Task<bool> DeleteUserImg(string username);
    }
}
