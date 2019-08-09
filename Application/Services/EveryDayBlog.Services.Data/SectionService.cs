namespace EveryDayBlog.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Common.Repositories;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Mapping;
    using EveryDayBlog.Web.ViewModels.Sections.InputModels;
    using Microsoft.EntityFrameworkCore;

    public class SectionService : ISectionService
    {
        private readonly IDeletableEntityRepository<Section> sections;

        public SectionService(IDeletableEntityRepository<Section> sections)
        {
            this.sections = sections;
        }

        public async Task<int> CreateSectionAsync(SectionInputModel sectionInputModel, int postId)
        {
            // TODO :MANY-TO-MANY POSTS SECTIONS

            var sectionForDb = new Section
            {
                CreatedOn = DateTime.UtcNow,
                Content = sectionInputModel.SectionContent,
                Title = sectionInputModel.SectionTitle,
                //PostId = postId,
            };


            await this.sections.AddAsync(sectionForDb);

            sectionForDb.SectionPosts.Add(new SectionPost { PostId = postId });

            await this.sections.SaveChangesAsync();


            return sectionForDb.Id;
        }

        public async Task<Section> CreateSectionServiceOnlyAsync(SectionInputModel sectionInputModel)
        {
            var sectionForDb = new Section
            {
                CreatedOn = DateTime.UtcNow,
                Content = sectionInputModel.SectionContent,
                Title = sectionInputModel.SectionTitle,
            };

            await this.sections.AddAsync(sectionForDb);

            await this.sections.SaveChangesAsync();


            return sectionForDb;
        }

        public async Task<TEntity> GetSectionByIdAsync<TEntity>(int sectionId)
        {
            return await this.sections.All()
                 .Where(s => s.Id == sectionId)
                 .To<TEntity>()
                 .SingleOrDefaultAsync();
        }
    }
}

