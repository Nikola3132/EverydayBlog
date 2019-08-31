namespace EveryDayBlog.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using EveryDayBlog.Common;
    using EveryDayBlog.Data.Common.Repositories;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Mapping;
    using EveryDayBlog.Web.ViewModels.PageHeaders.InputModels;
    using Microsoft.EntityFrameworkCore;

    public class PageHeaderService : IPageHeaderService
    {
        private readonly IDeletableEntityRepository<PageHeader> pageHeaders;
        private readonly ICloudinaryService cloudinaryService;

        public PageHeaderService(
            IDeletableEntityRepository<PageHeader> pageHeaders,
            ICloudinaryService cloudinaryService)
        {
            this.pageHeaders = pageHeaders;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<int> CreatePageHeaderAsync(PageHeaderInputModel pageHeaderInputModel)
        {
            var pageHeaderImg = pageHeaderInputModel.Image;

            var pageHeaderForDb = new PageHeader
            {
                CreatedOn = DateTime.UtcNow,
                PageIndicator = GlobalConstants.PageIndicatorPost,
                Title = pageHeaderInputModel.MainTitle,
                SubTitle = pageHeaderInputModel.SubTitle,
            };

            if (pageHeaderImg != null)
            {
                var folderName = GlobalConstants.PageHeadersFolderName;

                var cloudUrl = this.cloudinaryService.UploudPicture(pageHeaderImg, folderName);
                pageHeaderForDb.Image = new Image
                {
                    CloudUrl = cloudUrl,
                    ContentType = pageHeaderImg.ContentType,
                    CreatedOn = DateTime.UtcNow,
                    ImageByte = pageHeaderImg.ImageByte,
                    ImagePath = pageHeaderImg.ImagePath,
                    ImageTitle = pageHeaderImg.ImageTitle,
                };
            }

            await this.pageHeaders.AddAsync(pageHeaderForDb);
            var savedChanges = await this.pageHeaders.SaveChangesAsync();

            return pageHeaderForDb.Id;
        }

        public TEntity GetAdminPageHeadersByPageIndicatorAsync<TEntity>()
        {
            return this.pageHeaders.All()
                .Where(ph => ph.PageIndicator == GlobalConstants.AdministratorRoleName)
                .To<TEntity>()
                .FirstOrDefault();
        }

        public async Task<TEntity> GetPageHeaderById<TEntity>(int pageHeaderId)
        {
            return await this.pageHeaders.All()
                 .Where(ph => ph.Id == pageHeaderId)
                 .To<TEntity>()
                 .SingleOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetPageHeadersByPageIndicatorAsync<TEntity>(string pageIndicator)
        {
            return await this.pageHeaders.All()
                 .Where(ph => ph.PageIndicator == pageIndicator)
                 .To<TEntity>()
                 .ToListAsync<TEntity>();
        }

        public async Task<bool> UpdateAsync(int pageHeaderId, PageHeaderInputModel pageHeaderInputModel)
        {
            var pageHeader = await this.pageHeaders.All().SingleOrDefaultAsync(ph => ph.Id == pageHeaderId);

            var pageHeaderImg = pageHeaderInputModel.Image;

            pageHeader.ModifiedOn = DateTime.UtcNow;
            pageHeader.Title = pageHeaderInputModel.MainTitle;
            pageHeader.SubTitle = pageHeaderInputModel.SubTitle;

            if (pageHeaderImg != null)
            {
                var folderName = GlobalConstants.PageHeadersFolderName;

                var cloudUrl = this.cloudinaryService.UploudPicture(pageHeaderImg, folderName);
                pageHeader.Image = new Image
                {
                    CloudUrl = cloudUrl,
                    ContentType = pageHeaderImg.ContentType,
                    CreatedOn = DateTime.UtcNow,
                    ImageByte = pageHeaderImg.ImageByte,
                    ImagePath = pageHeaderImg.ImagePath,
                    ImageTitle = pageHeaderImg.ImageTitle,
                };
            }

            this.pageHeaders.Update(pageHeader);
            var savedChanges = await this.pageHeaders.SaveChangesAsync();

            return savedChanges > 0;
        }
    }
}
