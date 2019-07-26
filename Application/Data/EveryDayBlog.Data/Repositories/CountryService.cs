using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using EveryDayBlog.Web.ViewModels.Contries.ViewModels;

namespace EveryDayBlog.Data.Repositories
{
    public class CountryService : ICountryService
    {
        private readonly ApplicationDbContext dbContext;

        public CountryService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<CountryRegisterViewModel> GetAllConties()
        {

            // Not using AutoMapper ,because its not repository 
            return this.dbContext.Countries
                .Select(c => new CountryRegisterViewModel { Code = c.Code, Name = c.Name })
                .ToList();
        }
    }
}
