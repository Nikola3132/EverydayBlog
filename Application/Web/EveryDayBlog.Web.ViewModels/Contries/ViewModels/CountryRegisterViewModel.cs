using System;
using System.Collections.Generic;
using System.Text;
using EveryDayBlog.Data.Models;
using EveryDayBlog.Services.Mapping;

namespace EveryDayBlog.Web.ViewModels.Contries.ViewModels
{
    public class CountryRegisterViewModel : IMapFrom<Country>
    {
        public string Name { get; set; }

        public string Code { get; set; }
    }
}
