namespace EveryDayBlog.Web.Infrastructure.ModelBinders
{
    using System;
    using System.Threading.Tasks;

    using EveryDayBlog.Web.Infrastructure.Extensions;
    using EveryDayBlog.Web.Infrastructure.Models;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.DependencyInjection;

    public class TempDataSerializedToObjectModelBinder<TViewModel> : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var tempDataDictionaryFactory = bindingContext.HttpContext.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
            var tempDataDictionary = tempDataDictionaryFactory.GetTempData(bindingContext.HttpContext);

            var emailViewModel = tempDataDictionary.Get<EmailViewModel>("EmailOptions");

            bindingContext.Result = ModelBindingResult.Success(emailViewModel);
        }
    }
}
