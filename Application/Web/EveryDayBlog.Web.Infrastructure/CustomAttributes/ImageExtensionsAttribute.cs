namespace EveryDayBlog.Web.Infrastructure.CustomAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using EveryDayBlog.Data.Models;
    using Microsoft.AspNetCore.Http;

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
                var fileName = file.ImageTitle;

                return this.AllowedExtensions.Any(y => fileName.EndsWith(y));
            }

            return true;
        }
    }
}
