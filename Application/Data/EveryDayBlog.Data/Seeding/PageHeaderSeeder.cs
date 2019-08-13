using EveryDayBlog.Common;
using EveryDayBlog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveryDayBlog.Data.Seeding
{
    public class PageHeaderSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.PagesHeaders.Any(ph => ph.PageIndicator == GlobalConstants.AdministratorRoleName))
            {
                return;
            }

            var background = dbContext.Images.FirstOrDefault(i => i.ImageTitle == "AdminBackground");
            await dbContext.PagesHeaders.AddAsync(new PageHeader { CreatedOn = DateTime.UtcNow, SubTitle = string.Empty, Image = background, PageIndicator = GlobalConstants.AdministratorRoleName, Title = "" });
        }
    }
}
