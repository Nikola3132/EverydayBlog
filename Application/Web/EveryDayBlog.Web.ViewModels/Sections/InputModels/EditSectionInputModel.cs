using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayBlog.Web.ViewModels.Sections.InputModels
{
    public class EditSectionInputModel
    {

        private const string TitleErrorMsg = "Your {0} cannot be with more than {1} and lower than {2} symbols";
        private const string ContentErrorMsg = "Your {0} cannot be lower than {1}";

        public int Id { get; set; }


        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3, ErrorMessage = TitleErrorMsg)]
        [DataType(DataType.Text)]
        [Display(Name = "title")]
        public string SectionTitle { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = ContentErrorMsg)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "content")]
        public string SectionContent { get; set; }
    }
}
