namespace EveryDayBlog.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EveryDayBlog.Web.ViewModels.UsersRequests.InputModels;

    public interface IUserRequestService
    {
        Task<bool> SendQuestionAsync(UserRequestInputModel userRequestInputModel);

        Task<bool> MarkAsReadedAsync(int userRequestId);

        Task<bool> SoftDeleteAsync(int userRequestId);

        Task<bool> HardDeleteAsync(int userRequestId);

        Task<TEntity> TakeUserRequestById<TEntity>(int userRequestId);

        Task<List<TEntity>> TakeAllRequests<TEntity>();

        Task<List<TEntity>> TakeAllNonDeletedRequests<TEntity>();

        Task<List<TEntity>> TakeAllDeletedRequests<TEntity>();

        Task<bool> AnyDeletedUserRequests();

        Task<List<TEntity>> TakeAllNonReadedRequests<TEntity>();

        Task<List<TEntity>> TakeAllReadedRequests<TEntity>();

        int TakeAllReadedRequestsCount();

        int TakeAllRequestsCount();

        int TakeAllUnReadedRequestsCount();




    }
}
