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

        public async Task<TEntity> GetPageHeaderById<TEntity>(int pageHeaderId)
        {
           return await this.pageHeaders.All()
                .Where(ph => ph.Id == pageHeaderId)
                .To<TEntity>()
                .SingleOrDefaultAsync();
        }
    }
}
