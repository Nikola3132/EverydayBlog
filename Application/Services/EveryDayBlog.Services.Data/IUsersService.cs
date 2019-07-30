namespace EveryDayBlog.Services.Data
{
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Web.ViewModels.Images.InputModels;

    public interface IUsersService
    {
        Task<TEntity> GetUserByUsernameAsync<TEntity>(string username);

        Task<string> GetUserImageIfExistsAsync(string username);

        Task<bool> AddUserImageAsync(ImageInputModel imageInputModel, string username);

        Task<bool> DeleteUserImgAsync(string username);
    }
}
