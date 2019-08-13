using EveryDayBlog.Common;
using EveryDayBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveryDayBlog.Data.Seeding
{
    public class ImageSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Images.Any(i => i.CloudUrl == GlobalConstants.ImgAdminCloudUrl))
            {
                return;
            }

            await dbContext.Images.AddAsync(new Image { CloudUrl = GlobalConstants.ImgAdminCloudUrl, CreatedOn = DateTime.UtcNow, ContentType = "", ImagePath = "", ImageTitle = "AdminBackground" });
        }
    }
}
