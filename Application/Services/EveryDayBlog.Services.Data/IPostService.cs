namespace EveryDayBlog.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using EveryDayBlog.Web.ViewModels.Posts.InputModels;
    using EveryDayBlog.Web.ViewModels.Posts.ViewModels;
    using EveryDayBlog.Web.ViewModels.Sections.InputModels;

    public interface IPostService
    {
        Task<bool> CreatePostAsync(PostInputModel postInputModel, string username);

        Task<bool> AddSectionToPostAsync(int postId, SectionInputModel sectionInputModel);

        Task<TEntity> GetPostByIdAsync<TEntity>(int postId);


    }
}
