using EveryDayBlog.Data.Models;
using EveryDayBlog.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayBlog.Web.ViewModels.Images.ViewModels
{
    public class ImageBackgroundViewModel : IMapFrom<Image>
    {
        public string CloudUrl { get; set; }
    }
}
