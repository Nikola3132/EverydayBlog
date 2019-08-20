namespace EveryDayBlog.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EveryDayBlog.Web.ViewModels.PageHeaders.InputModels;

    public interface IPageHeaderService
    {
        Task<int> CreatePageHeaderAsync(PageHeaderInputModel pageHeaderInputModel);


        Task<TEntity> GetPageHeaderById<TEntity>(int pageHeaderId);

        Task<List<TEntity>> GetPageHeadersByPageIndicatorAsync<TEntity>(string pageIndicator);

        TEntity GetAdminPageHeadersByPageIndicatorAsync<TEntity>();
        Task<bool> UpdateAsync(int pageHeaderId, PageHeaderInputModel pageHeaderInputModel);
    }
}
