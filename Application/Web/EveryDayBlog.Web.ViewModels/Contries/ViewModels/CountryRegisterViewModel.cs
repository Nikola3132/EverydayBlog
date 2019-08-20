namespace EveryDayBlog.Web.ViewModels.Contries.ViewModels
{
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Mapping;

    public class CountryRegisterViewModel : IMapFrom<Country>
    {
        public string Name { get; set; }

        public string Code { get; set; }
    }
}
