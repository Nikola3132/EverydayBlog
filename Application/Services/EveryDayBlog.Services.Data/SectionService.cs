namespace EveryDayBlog.Services.Data
{
    using System;
    using System.Collections.Generic;
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

        public async Task<List<TEntity>> AllDeletedSections<TEntity>()
        {
            return await this.sections.AllWithDeleted().Where(s => s.IsDeleted).OrderBy(s => s.Id).To<TEntity>().ToListAsync();
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

        public async Task<bool> HardDelete(int sectionId)
        {
            var deletedSection = await this.sections.AllWithDeleted().SingleOrDefaultAsync(s => s.Id == sectionId && s.IsDeleted);
            if (deletedSection == null)
            {
                return false;
            }

            this.sections.HardDelete(deletedSection);
            return await this.sections.SaveChangesAsync() > 0;
        }

        public async Task<bool> ReorganizeAsync(int sectionId)
        {
            var section = await this.sections.AllWithDeleted().SingleOrDefaultAsync(s => s.Id == sectionId && s.IsDeleted == true);

            section.IsDeleted = false;

            this.sections.Update(section);
           return await this.sections.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDelete(int sectionId)
        {
           var currentSection = await this.sections.All().SingleOrDefaultAsync(s => s.Id == sectionId);
           if (currentSection == null)
            {
                return false;
            }

           this.sections.Delete(currentSection);
           return await this.sections.SaveChangesAsync() > 0;

        }

        public async Task<bool> UpdateSectionByIdAsync(int sectionId, EditSectionInputModel modifiedSection)
        {
           var sectionUpdate = await this.sections.All().SingleOrDefaultAsync(s => s.Id == sectionId);

           sectionUpdate.Content = modifiedSection.SectionContent;
           sectionUpdate.Title = modifiedSection.SectionTitle;

           this.sections.Update(sectionUpdate);

           return await this.sections.SaveChangesAsync() > 0;
        }
    }
}

