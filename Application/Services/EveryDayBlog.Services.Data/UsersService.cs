namespace EveryDayBlog.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using EveryDayBlog.Common;
    using EveryDayBlog.Data.Common.Repositories;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Mapping;
    using EveryDayBlog.Web.Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> users;
        private readonly IDeletableEntityRepository<Image> images;
        private readonly ICloudinaryService cloudinaryService;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> users,
            IDeletableEntityRepository<Image> images,
            ICloudinaryService cloudinaryService)
        {
            this.users = users;
            this.images = images;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<TEntity> GetUserByUsernameAsync<TEntity>(string username)
        {
            var currentUser = this.users
                .All()
                .Where(u => u.UserName == username)
                .To<TEntity>().SingleOrDefaultAsync();

            return await currentUser;
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            var currentUser = this.users
               .All()
               .SingleOrDefaultAsync(u => u.UserName == username);

            return await currentUser;
        }

        public async Task<string> GetUserImageIfExistsAsync(string username)
        {
             var currentUser = await this.users.All().Where(u => u.UserName == username)
                .Include(u => u.Image).SingleOrDefaultAsync();

             return currentUser.Image?.CloudUrl;
        }

        public async Task<bool> AddUserImageAsync(ImageInputModel imageInputModel, string username)
        {
            var currentUser = await this.users.All().SingleOrDefaultAsync(u => u.UserName == username);
            var cloudUrl = this.cloudinaryService.UploudPicture(imageInputModel, GlobalConstants.UsersFolderName);

            var imgForDb = new Image
            {
                ContentType = imageInputModel.ContentType,
                ImagePath = imageInputModel.ImagePath,
                ImageTitle = imageInputModel.ImageTitle,
                CloudUrl = cloudUrl,
            };
            currentUser.Image = imgForDb;


            int addedImgs = await this.users.SaveChangesAsync();
            return addedImgs == 1;
        }

        public async Task<bool> DeleteUserImgAsync(string email)
        {
            var currentUser = await this.users.All().Include(u => u.Image).SingleOrDefaultAsync(u => u.Email == email);

            currentUser.ImageId = null;

            this.images.Delete(currentUser.Image);

            currentUser.Image = null;

            int deletedUsers = await this.users.SaveChangesAsync();
            int deletedImgs = await this.images.SaveChangesAsync();

            if (deletedUsers == 1 && deletedImgs == 1)
            {
                return true;
            }

            return false;

        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await this.users.All().SingleOrDefaultAsync(u => u.Id == id);
        }
    }
}
