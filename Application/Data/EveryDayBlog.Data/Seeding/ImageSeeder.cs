namespace EveryDayBlog.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using EveryDayBlog.Common;
    using EveryDayBlog.Data.Models;

    public class ImageSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Images.Any(i => i.CloudUrl == GlobalConstants.ImgAdminCloudUrl))
            {
                await dbContext.Images.AddAsync(new Image { CloudUrl = GlobalConstants.ImgAdminCloudUrl, CreatedOn = DateTime.UtcNow, ContentType = string.Empty, ImagePath = string.Empty, ImageTitle = "AdminBackground" });
            }

            if (!dbContext.Images.Any(i => i.CloudUrl == GlobalConstants.ImgAboutCloudUrl))
            {
               await dbContext.Images.AddAsync(new Image { CloudUrl = GlobalConstants.ImgAboutCloudUrl, CreatedOn = DateTime.UtcNow, ContentType = string.Empty, ImagePath = string.Empty, ImageTitle = GlobalConstants.About });
            }

            if (!dbContext.Images.Any(i => i.CloudUrl == GlobalConstants.ImgHomeCloudUrl))
            {
                await dbContext.Images.AddAsync(new Image { CloudUrl = GlobalConstants.ImgHomeCloudUrl, CreatedOn = DateTime.UtcNow, ContentType = string.Empty, ImagePath = string.Empty, ImageTitle = GlobalConstants.Home });
            }

            if (!dbContext.Images.Any(i => i.CloudUrl == GlobalConstants.ImgContactCloudUrl))
            {
                await dbContext.Images.AddAsync(new Image { CloudUrl = GlobalConstants.ImgContactCloudUrl, CreatedOn = DateTime.UtcNow, ContentType = string.Empty, ImagePath = string.Empty, ImageTitle = GlobalConstants.Contact });
            }

            if (!dbContext.Images.Any(i => i.CloudUrl == GlobalConstants.ImgProfileCloudUrl))
            {
                await dbContext.Images.AddAsync(new Image { CloudUrl = GlobalConstants.ImgProfileCloudUrl, CreatedOn = DateTime.UtcNow, ContentType = string.Empty, ImagePath = string.Empty, ImageTitle = GlobalConstants.Profile });
            }

            if (!dbContext.Images.Any(i => i.CloudUrl == GlobalConstants.ImgPostCreateCloudUrl))
            {
                await dbContext.Images.AddAsync(new Image { CloudUrl = GlobalConstants.ImgPostCreateCloudUrl, CreatedOn = DateTime.UtcNow, ContentType = string.Empty, ImagePath = string.Empty, ImageTitle = GlobalConstants.PostCreate });
            }

            if (!dbContext.Images.Any(i => i.CloudUrl == GlobalConstants.ImgRegistrationCloudUrl))
            {
                await dbContext.Images.AddAsync(new Image { CloudUrl = GlobalConstants.ImgRegistrationCloudUrl, CreatedOn = DateTime.UtcNow, ContentType = string.Empty, ImagePath = string.Empty, ImageTitle = GlobalConstants.Registration });
            }

            if (!dbContext.Images.Any(i => i.CloudUrl == GlobalConstants.ImgLoginCloudUrl))
            {
                await dbContext.Images.AddAsync(new Image { CloudUrl = GlobalConstants.ImgLoginCloudUrl, CreatedOn = DateTime.UtcNow, ContentType = string.Empty, ImagePath = string.Empty, ImageTitle = GlobalConstants.Login });
            }
        }
    }
}
