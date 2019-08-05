namespace EveryDayBlog.Services.Data
{
    using System.Threading.Tasks;

    using EveryDayBlog.Web.ViewModels.PageHeaders.InputModels;

    public interface IPageHeaderService
    {
        Task<int> CreatePageHeaderAsync(PageHeaderInputModel pageHeaderInputModel);

        Task<TEntity> GetPageHeaderById<TEntity>(int pageHeaderId);

    }
}
