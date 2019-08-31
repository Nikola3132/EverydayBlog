namespace EveryDayBlog.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EveryDayBlog.Common;
    using EveryDayBlog.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class UserSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var users = dbContext.Users.ToList();

            bool exists = false;

            foreach (var user in users)
            {
                if (await userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName))
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                var user = new ApplicationUser
                {
                    Email = "admin@gmail.com",
                    UserName = "admin@gmail.com",
                    EmailConfirmed = true,
                };

                await userManager.CreateAsync(user, "123456");
                await dbContext.SaveChangesAsync();

                await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
