namespace EveryDayBlog.Web.Infrastructure.CustomAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using EveryDayBlog.Web.Infrastructure.Models;

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
            var file = value as ImageInputModel;

            if (file != null)
            {
                var fileName = file.ImageTitle.ToLower();

                return this.AllowedExtensions.Any(y => fileName.EndsWith(y));
            }

            return true;
        }
    }
}
