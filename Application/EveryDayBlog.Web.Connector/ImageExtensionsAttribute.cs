using System;

namespace EveryDayBlog.Web.Connector
{
    using EveryDayBlog.Data.Models;
    using System;
        using System.Collections.Generic;
        using System.ComponentModel.DataAnnotations;
        using System.Linq;

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
        public class ImageExtensionsAttribute : ValidationAttribute
        {
            public ImageExtensionsAttribute(string fileExtensions)
            {
                this.AllowedExtensions = fileExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            private List<string> AllowedExtensions { get; set; }

            public override bool IsValid(object value)
            {
                var file = value as Image;

                if (file != null)
                {
                    var fileName = file.ImageTitle.ToLower();

                    return this.AllowedExtensions.Any(y => fileName.EndsWith(y));
                }

                return true;
            }
        }
    

}
