namespace EveryDayBlog.Services.Data
{
    using System.Threading.Tasks;

    using EveryDayBlog.Web.ViewModels.UsersRequests.InputModels;

    public interface IUserRequestService
    {
        Task<bool> SendQuestionAsync(UserRequestInputModel userRequestInputModel);

        Task<bool> MarkAsReadedAsync(int userRequestId);

        Task<bool> SoftDeleteAsync(int userRequestId);

        Task<bool> HardDeleteAsync(int userRequestId);

        Task<TEntity> TakeUserRequestById<TEntity>(int userRequestId);



    }
}
