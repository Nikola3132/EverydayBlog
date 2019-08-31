namespace EveryDayBlog.Services.Data.Tests.Config
{
    using System.Reflection;

    using EveryDayBlog.Services.Data.Tests.ViewModels;
    using EveryDayBlog.Services.Mapping;
    using EveryDayBlog.Web.Areas.Administration.ViewModels.Messages.ViewModel;
    using EveryDayBlog.Web.ViewModels;

    public class InitilizeAutoMapper
    {
        public static void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(
               typeof(SectionViewTestModel).GetTypeInfo().Assembly);
            //typeof(ErrorViewModel).GetTypeInfo().Assembly,
            //typeof(DeletedMessagesViewModel).GetTypeInfo().Assembly
        }
    }
}
