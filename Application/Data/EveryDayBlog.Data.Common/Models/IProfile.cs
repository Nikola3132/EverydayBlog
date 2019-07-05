namespace EveryDayBlog.Data.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public interface IProfile
    {
         string FirstName { get; set; }

         string LastName { get; set; }

         string Description { get; set; }

    }
}
