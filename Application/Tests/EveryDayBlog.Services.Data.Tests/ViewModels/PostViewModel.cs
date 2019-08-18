using EveryDayBlog.Data.Models;
using EveryDayBlog.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayBlog.Services.Data.Tests.ViewModels
{
    public class PostViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public PageHeaderViewModel PageHeader { get; set; }
    }
}
