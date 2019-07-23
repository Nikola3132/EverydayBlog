namespace EveryDayBlog.Web.Infrastructure.ModelBinders
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Extensions;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class FileToImageModelBinder : IModelBinder
    {

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult =
            bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                var imgFormFile = bindingContext.HttpContext.Request.Form.Files.FirstOrDefault();

                if (imgFormFile != null)
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = new MemoryStream())
                    {
                        await imgFormFile.CopyToAsync(stream);
                        bindingContext.Result = ModelBindingResult.Success(new Image
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
}
