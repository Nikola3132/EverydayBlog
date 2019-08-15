using EveryDayBlog.Web.ViewModels.PageHeaders.InputModels;
using EveryDayBlog.Web.ViewModels.Sections.InputModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace EveryDayBlog.Web.Areas.Administration.ViewModels.Posts.InputModels
{
    public class EditPostInputModel
    {
        [BindNever]
        public int Id { get; set; }

        [BindProperty]
        public PageHeaderInputModel PageHeader { get; set; }
        = new PageHeaderInputModel();

        [BindProperty]
        public List<EditSectionInputModel> Sections { get; set; }
        = new List<EditSectionInputModel>();
    }
}
