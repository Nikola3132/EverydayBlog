using EveryDayBlog.Data.Models;
using EveryDayBlog.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayBlog.Services.Data.Tests.ViewModels
{
    public class PageHeaderViewModel : IMapFrom<PageHeader>
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }
    }
}
