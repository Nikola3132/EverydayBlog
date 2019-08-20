namespace EveryDayBlog.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using EveryDayBlog.Web.ViewModels.Contries.ViewModels;

    public class CountryService : ICountryService
    {
        private readonly ApplicationDbContext dbContext;

        public CountryService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<CountryRegisterViewModel> GetAllConties()
        {
            return this.dbContext.Countries
                .Select(c => new CountryRegisterViewModel { Code = c.Code, Name = c.Name })
                .ToList();
        }
    }
}
