namespace EveryDayBlog.Web.Infrastructure.ModelBinders
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Web.Infrastructure.Models;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class FileToImageModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var imgFormFile = bindingContext.HttpContext.Request.Form.Files.FirstOrDefault();

            if (imgFormFile != null)
            {
                var filePath = Path.GetTempFileName();

                using (var stream = new MemoryStream())
                {
                    await imgFormFile.CopyToAsync(stream);
                    bindingContext.Result = ModelBindingResult.Success(new ImageInputModel
                    {
                        CreatedOn = DateTime.UtcNow,
                        ImageByte = stream.ToArray(),
                        ImagePath = filePath,
                        ImageTitle = imgFormFile.FileName,
                        ContentType = imgFormFile.ContentType,
                    });
                }
            }
        }
    }
}
