﻿namespace EveryDayBlog.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Common.Repositories;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Data.Models.Enums;
    using EveryDayBlog.Services.Mapping;
    using EveryDayBlog.Web.ViewModels.Posts.InputModels;
    using EveryDayBlog.Web.ViewModels.Posts.ViewModels;
    using EveryDayBlog.Web.ViewModels.Sections.InputModels;
    using Microsoft.EntityFrameworkCore;

    public class PostService : IPostService
    {
        private readonly IDeletableEntityRepository<Post> posts;
        private readonly IUsersService usersService;
        private readonly IPageHeaderService pageHeaderService;
        private readonly ISectionService sectionService;

        public PostService(
            IDeletableEntityRepository<Post> posts,
            IUsersService usersService,
            IPageHeaderService pageHeaderService,
            ISectionService sectionService
            )
        {
            this.posts = posts;
            this.usersService = usersService;
            this.pageHeaderService = pageHeaderService;
            this.sectionService = sectionService;
        }

        public async Task<bool> AddSectionToPostAsync(int postId, SectionInputModel sectionInputModel)
        {
            var currentPost = await this.GetPostByIdAsync<Post>(postId);

            var sectionForAdding = await this.sectionService.CreateSectionServiceOnlyAsync(sectionInputModel);

            currentPost.Sections.Add(sectionForAdding);

            int savedChanges = await this.posts.SaveChangesAsync();

            return savedChanges > 0;
        }

        public async Task<bool> CreatePostAsync(PostInputModel postInputModel, string username)
        {
            var currentUser = await this.usersService.GetUserByUsernameAsync(username);

            int pageHeaderId = await this.pageHeaderService.CreatePageHeaderAsync(postInputModel.PageHeader);

            var section = await this.sectionService.CreateSectionServiceOnlyAsync(postInputModel.Section);

            var postForDb = new Post
            {
                CreatedOn = DateTime.UtcNow,
                User = currentUser,
                UserId = currentUser.Id,
                PageHeaderId = pageHeaderId,
            };

            postForDb.Sections.Add(section);

            await this.posts.AddAsync(postForDb);

            return await this.posts.SaveChangesAsync() > 0;
        }

        public async Task<TEntity> GetPostByIdAsync<TEntity>(int postId)
        {
            return await this.posts.All()
                .Where(p => p.Id == postId)
                .To<TEntity>()
                .SingleOrDefaultAsync();
        }

        public IEnumerable<TEntity> GetPostsBySearch<TEntity>(string searchString)
        {
            var searchStringClean = searchString
                .Split(
                    new string[]
                {
                    ",", ".", " ",
                }, StringSplitOptions.RemoveEmptyEntries);

            IQueryable<Post> posts = this.posts
                .All()
                .Include(p => p.PageHeader)
                .Where(x => searchStringClean
                .All(c => x.PageHeader.Title.ToLower().Contains(c.ToLower())));

            return posts.To<TEntity>();
        }

        public IEnumerable<TEntity> GetPostsFilter<TEntity>(string searchString)
        {
            if (searchString != null)
            {
                return this.GetPostsBySearch<TEntity>(searchString);
            }

            return this.GetVisiblePosts<TEntity>();
        }

        public IEnumerable<TEntity> GetVisiblePosts<TEntity>()
        {
            return this.posts.All()
                             .Include(p => p.PageHeader)
                             .ThenInclude(x => x.Image)
                             .Include(x => x.User)
                             .To<TEntity>();
        }

        public IEnumerable<IndexPostViewModel> OrderBy(IEnumerable<IndexPostViewModel> posts, PostsSort sortBy)
        {
            if (sortBy == PostsSort.Oldest)
            {
                return posts.OrderBy(p => p.PostedBy).ToList();
            }

            // ProductsSortType.Newest
            return posts.OrderByDescending(p => p.PostedBy).ToList();
        }

        public Post GetProductById(int id)
        {
            return this.posts.All().Include(p => p.PageHeader)
                                   .Include(x => x.User)
                                   .FirstOrDefault(x => x.Id == id);
        }
    }
}
