using EveryDayBlog.Services.Extensions;
using EveryDayBlog.Web.ViewModels.Emails.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EveryDayBlog.Web.Infrastructure.ModelBinders
{
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

            var emailViewModel = TempDataExtensions.Get<EmailViewModel>(tempDataDictionary, "EmailOptions");

            bindingContext.Result = ModelBindingResult.Success(emailViewModel);
            }
        }
    }
