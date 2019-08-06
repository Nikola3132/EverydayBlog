using EveryDayBlog.Data.Models;
using EveryDayBlog.Services.Mapping;
using EveryDayBlog.Web.ViewModels.Images.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayBlog.Web.ViewModels.PageHeaders.ViewModels
{
    public class PageHeaderViewModel : IMapFrom<PageHeader>
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public ImageBackgroundViewModel Image { get; set; }
    }
}
