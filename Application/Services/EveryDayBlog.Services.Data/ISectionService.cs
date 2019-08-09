namespace EveryDayBlog.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Web.ViewModels.Sections.InputModels;

    public interface ISectionService
    {
        Task<int> CreateSectionAsync(SectionInputModel sectionInputModel, int postId);

        Task<Section> CreateSectionServiceOnlyAsync(SectionInputModel sectionInputModel);


        Task<TEntity> GetSectionByIdAsync<TEntity>(int sectionId);

    }
}
