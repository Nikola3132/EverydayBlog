namespace EveryDayBlog.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Data.Models.Enums;
    using EveryDayBlog.Web.ViewModels.Posts.InputModels;
    using EveryDayBlog.Web.ViewModels.Posts.ViewModels;
    using EveryDayBlog.Web.ViewModels.Sections.InputModels;

    public interface IPostService
    {
        Task<bool> CreatePostAsync(PostInputModel postInputModel, string username);

        Task<bool> AddSectionToPostAsync(int postId, SectionInputModel sectionInputModel);

        Task<TEntity> GetPostByIdAsync<TEntity>(int postId);

        IEnumerable<TEntity> GetPostsFilter<TEntity>(string searchString);

        IEnumerable<TEntity> GetPostsBySearch<TEntity>(string searchString);

        IEnumerable<TEntity> GetVisiblePosts<TEntity>();

        Task<IEnumerable<IndexPostViewModel>> OrderByAsync(IEnumerable<IndexPostViewModel> posts, PostsSort sortBy, string username = null);

        Task<bool> HidePostById(int postId);

        Task<int> GetPageHeaderIdAsync(int postId);

        Task<string> GetCreatorsIdAsync(int postId);

    }
}
