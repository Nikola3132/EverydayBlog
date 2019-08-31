namespace EveryDayBlog.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EveryDayBlog.Common;
    using EveryDayBlog.Data.Models;

    public class PageHeaderSeeder : ISeeder
    {
        private const string AdminSubTitle = "Responsible And Committed To What He Does";
        private const string HomeSubTitleValue = "Interested to know why and what we are?";
        private const string HomeSubTitle = "Мake yourself at home";
        private const string ContactSubTitle = "If you have any questions or suggestions on how to improve something, do not hesitate to contact us";
        private const string ProfileSubTitle = "If you made a mistake or want to change the information you gave to us, here's the place to do it";
        private const string PostCreateSubTitle = "One day, I will find the inspiration that will carry me to the end of the writing career rainbow.";
        private const string PostCreateTitle = "Create Post";
        private const string RegistrationSubTitle = "Create your own account and be one of us.";
        private const string LoginSubTitle = "Let us know that you are real.";

        private const string ImgTitleForAdmin = "AdminBackground";

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.PagesHeaders.Any(ph => ph.PageIndicator == GlobalConstants.AdministratorRoleName))
            {
                var background = dbContext.Images.FirstOrDefault(i => i.ImageTitle == ImgTitleForAdmin);
                await dbContext.PagesHeaders.AddAsync(new PageHeader { CreatedOn = DateTime.UtcNow, SubTitle = AdminSubTitle, Image = background, PageIndicator = GlobalConstants.AdministratorRoleName, Title = GlobalConstants.AdministratorRoleName });
            }

            if (!dbContext.PagesHeaders.Any(ph => ph.PageIndicator == GlobalConstants.About))
            {
                var background = dbContext.Images.FirstOrDefault(i => i.ImageTitle == GlobalConstants.About);
                await dbContext.PagesHeaders.AddAsync(new PageHeader { CreatedOn = DateTime.UtcNow, SubTitle = HomeSubTitleValue, Image = background, PageIndicator = GlobalConstants.About, Title = GlobalConstants.About });
            }

            if (!dbContext.PagesHeaders.Any(ph => ph.PageIndicator == GlobalConstants.Home))
            {
                var background = dbContext.Images.FirstOrDefault(i => i.ImageTitle == GlobalConstants.Home);
                await dbContext.PagesHeaders.AddAsync(new PageHeader { CreatedOn = DateTime.UtcNow, SubTitle = HomeSubTitle, Image = background, PageIndicator = GlobalConstants.Home, Title = GlobalConstants.Home });
            }

            if (!dbContext.PagesHeaders.Any(ph => ph.PageIndicator == GlobalConstants.Contact))
            {
                var background = dbContext.Images.FirstOrDefault(i => i.ImageTitle == GlobalConstants.Contact);
                await dbContext.PagesHeaders.AddAsync(new PageHeader { CreatedOn = DateTime.UtcNow, SubTitle = ContactSubTitle, Image = background, PageIndicator = GlobalConstants.Contact, Title = GlobalConstants.Contact });
            }

            if (!dbContext.PagesHeaders.Any(ph => ph.PageIndicator == GlobalConstants.Profile))
            {
                var background = dbContext.Images.FirstOrDefault(i => i.ImageTitle == GlobalConstants.Profile);
                await dbContext.PagesHeaders.AddAsync(new PageHeader { CreatedOn = DateTime.UtcNow, SubTitle = ProfileSubTitle, Image = background, PageIndicator = GlobalConstants.Profile, Title = GlobalConstants.Profile });
            }

            if (!dbContext.PagesHeaders.Any(ph => ph.PageIndicator == GlobalConstants.PostCreate))
            {
                var background = dbContext.Images.FirstOrDefault(i => i.ImageTitle == GlobalConstants.PostCreate);
                await dbContext.PagesHeaders.AddAsync(new PageHeader { CreatedOn = DateTime.UtcNow, SubTitle = PostCreateSubTitle, Image = background, PageIndicator = GlobalConstants.PostCreate, Title = PostCreateTitle });
            }

            if (!dbContext.PagesHeaders.Any(ph => ph.PageIndicator == GlobalConstants.Registration))
            {
                var background = dbContext.Images.FirstOrDefault(i => i.ImageTitle == GlobalConstants.Registration);
                await dbContext.PagesHeaders.AddAsync(new PageHeader { CreatedOn = DateTime.UtcNow, SubTitle = RegistrationSubTitle, Image = background, PageIndicator = GlobalConstants.Registration, Title = GlobalConstants.Registration });
            }

            if (!dbContext.PagesHeaders.Any(ph => ph.PageIndicator == GlobalConstants.Login))
            {
                var background = dbContext.Images.FirstOrDefault(i => i.ImageTitle == GlobalConstants.Login);
                await dbContext.PagesHeaders.AddAsync(new PageHeader { CreatedOn = DateTime.UtcNow, SubTitle = LoginSubTitle, Image = background, PageIndicator = GlobalConstants.Login, Title = GlobalConstants.Login });
            }
        }
    }
}
