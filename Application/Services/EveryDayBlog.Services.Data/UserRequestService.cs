namespace EveryDayBlog.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Common.Repositories;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Mapping;
    using EveryDayBlog.Web.ViewModels.UsersRequests.InputModels;
    using Microsoft.EntityFrameworkCore;

    public class UserRequestService : IUserRequestService
    {
        private readonly IDeletableEntityRepository<UserRequest> usersRequests;

        public UserRequestService(IDeletableEntityRepository<UserRequest> usersRequests)
        {
            this.usersRequests = usersRequests;
        }

        public async Task<bool> SoftDeleteAsync(int userRequestId)
        {
            var userRequest = await this.usersRequests.All().SingleOrDefaultAsync(ur => ur.Id == userRequestId);
            this.usersRequests.Delete(userRequest);

            return await this.usersRequests.SaveChangesAsync() > 0;
        }

        public async Task<bool> MarkAsReadedAsync(int userRequestId)
        {
            var userRequest = await this.usersRequests.All().SingleOrDefaultAsync(ur => ur.Id == userRequestId);

            userRequest.IsReaded = true;

            this.usersRequests.Update(userRequest);

            return await this.usersRequests.SaveChangesAsync() > 0;
        }

        public async Task<bool> SendQuestionAsync(UserRequestInputModel userRequestInputModel)
        {
           await this.usersRequests.AddAsync(new UserRequest
            {
                CreatedOn = DateTime.UtcNow,
                Email = userRequestInputModel.Email,
                Message = userRequestInputModel.Message,
                Name = userRequestInputModel.Name,
                Phone = userRequestInputModel.Phone,
            });

           return await this.usersRequests.SaveChangesAsync() > 0;
        }

        public async Task<bool> HardDeleteAsync(int userRequestId)
        {
            var userRequest = await this.usersRequests.All().SingleOrDefaultAsync(ur => ur.Id == userRequestId);
            this.usersRequests.HardDelete(userRequest);

            return await this.usersRequests.SaveChangesAsync() > 0;
        }

        public async Task<TEntity> TakeUserRequestById<TEntity>(int userRequestId)
        {
            return await this.usersRequests
                .All()
                .Where(ur => ur.Id == userRequestId)
                .To<TEntity>()
                .FirstOrDefaultAsync<TEntity>();
        }
    }
}
