namespace EveryDayBlog.Services.Data
{
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Web.Infrastructure.Models;

    public interface IUsersService
    {
        Task<TEntity> GetUserByUsernameAsync<TEntity>(string username);

        Task<ApplicationUser> GetUserByUsernameAsync(string username);

        Task<ApplicationUser> GetUserByIdAsync(string id);

        Task<string> GetUserImageIfExistsAsync(string username);

        Task<string> GetUserFullName(string username);


        Task<bool> AddUserImageAsync(ImageInputModel imageInputModel, string username);

        Task<bool> DeleteUserImgAsync(string username);
    }
}
