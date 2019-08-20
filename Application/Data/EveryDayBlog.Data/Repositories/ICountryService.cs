namespace EveryDayBlog.Data.Repositories
{
    using System.Collections.Generic;

    using EveryDayBlog.Web.ViewModels.Contries.ViewModels;

    public interface ICountryService
    {
        List<CountryRegisterViewModel> GetAllConties();
    }
}
