namespace EveryDayBlog.Web.ViewModels.Posts.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using EveryDayBlog.Common;
    using EveryDayBlog.Web.Infrastructure.CustomAttributes;
    using EveryDayBlog.Web.Infrastructure.ModelBinders;
    using EveryDayBlog.Web.Infrastructure.Models;
    using EveryDayBlog.Web.ViewModels.PageHeaders.InputModels;
    using EveryDayBlog.Web.ViewModels.Sections.InputModels;
    using Microsoft.AspNetCore.Mvc;

    public class PostInputModel
    {
        [BindProperty]
        public PageHeaderInputModel PageHeader { get; set; }
        = new PageHeaderInputModel();

        [BindProperty]

        public SectionInputModel Section { get; set; }
        = new SectionInputModel();
    }
}
