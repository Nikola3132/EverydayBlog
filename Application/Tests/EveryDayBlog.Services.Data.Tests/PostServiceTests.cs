namespace EveryDayBlog.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using EveryDayBlog.Data.Common.Repositories;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Data.Models.Enums;
    using EveryDayBlog.Services.Data.Tests.ViewModels;
    using EveryDayBlog.Web.Infrastructure.Models;
    using EveryDayBlog.Web.ViewModels.PageHeaders.InputModels;
    using EveryDayBlog.Web.ViewModels.Posts.InputModels;
    using EveryDayBlog.Web.ViewModels.Posts.ViewModels;
    using EveryDayBlog.Web.ViewModels.Sections.InputModels;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class PostServiceTests : BaseTest
    {
        private IDeletableEntityRepository<ApplicationUser> usersRepository;
        private IDeletableEntityRepository<Image> imagesRepository;
        private IDeletableEntityRepository<Post> postsRepository;
        private IDeletableEntityRepository<PageHeader> pageHeadersRepository;
        private IDeletableEntityRepository<Section> sectionRepository;

        private CloudinaryService cloudinaryService;

        private UsersService userService;
        private PostService postService;
        private PageHeaderService pageHeaderService;
        private SectionService sectionService;

        public PostServiceTests()
        {
            this.imagesRepository = this.GetImageRepository(this.GetTestImagecList()).Object;
            this.usersRepository = this.GetUserRepository(this.GetTestUsercList()).Object;
            this.postsRepository = this.GetPostsRepository(this.GetTestPostsList()).Object;
            this.pageHeadersRepository = this.GetPageHeadersRepository(this.GetTestPageHeadersList()).Object;
            this.sectionRepository = this.GetSectionRepository(this.GetTestSectionsList()).Object;

            Account cloudinaryAcc
               = new Account(
               "dy78wnfy2",
               "857437556699719",
               "jj3uew_U5wJPCm8Xn_LWYbbHDf8");

            Cloudinary cloudinaryUtility = new Cloudinary(cloudinaryAcc);
            this.cloudinaryService = new CloudinaryService(cloudinaryUtility);
            this.userService = new UsersService(this.usersRepository, this.imagesRepository, this.cloudinaryService);

            this.pageHeaderService = new PageHeaderService(this.pageHeadersRepository, this.cloudinaryService);
            this.sectionService = new SectionService(this.sectionRepository);
            this.postService = new PostService(this.postsRepository, this.userService, this.pageHeaderService, this.sectionService);
        }

        [Fact]
        public async Task AddSectionToPostAsyncShouldAddTheSectionToThePost()
        {
            // Arrange
            var sectionTitle = "dsfdfsdfs";
            var sectionContent = "fdssdf";
            var sectionInputModel = new SectionInputModel
            {
                SectionContent = sectionContent,
                SectionTitle = sectionTitle,
            };

            // Act
            await this.postService.AddSectionToPostAsync(1, sectionInputModel);
            var currentPost = this.postsRepository.AllWithDeleted().FirstOrDefault(p => p.Id == 1);

            // Assert
            var addedSection = currentPost.PostSections.Last()?.Section;
            Assert.True(addedSection?.Title == sectionTitle && addedSection?.Content == sectionContent);
        }

        [Fact]
        public async Task CreatePostAsyncShouldReturnExpectedId()
        {
            // Arrange
            var expectedId = 11;
            byte[] img = await File.ReadAllBytesAsync($@"C:\Users\nikolaviktor3132\Pictures\test.png");

            PostInputModel expectedSection = new PostInputModel()
            {
                PageHeader = new PageHeaderInputModel
                {
                    Image = new ImageInputModel
                    {
                        CloudUrl = "test",
                        ContentType = "sadasd",
                        CreatedOn = DateTime.Now,
                        ImageByte = img,
                        ImageTitle = "weqeqweq",
                        ImagePath = "dsfsdfsdfs",
                    },
                    MainTitle = "dfdf",
                    SubTitle = "sdfsdfsd",
                },
                Section = new SectionInputModel
                {
                    SectionContent = "fdssdf",
                    SectionTitle = "dsfdfsdfs",
                },
            };
            var username = "first@gmail.com";
            var userReciver = await this.userService.GetUserByUsernameAsync(username);

            // Act
            await this.postService.CreatePostAsync(expectedSection, userReciver?.UserName);

            // Assert
            var actualSection = this.postsRepository.AllWithDeleted().Last();
            Assert.True(actualSection.Id == expectedId && actualSection.UserId == userReciver.Id);
        }

        [Fact]
        public async Task GetPostByIdAsyncShouldReturnExpectedPost()
        {
            // Arrange
            var expectedPost = new Post
            {
                Id = 2,
                PageHeader = new PageHeader { Title = "Test2" },
                User = new ApplicationUser { },
                PostSections = new List<SectionPost>(),
            };

            // Act
            var result = await this.postService.GetPostByIdAsync<PostViewTestModel>(expectedPost.Id);

            // Assert
            Assert.True(expectedPost.Id == result.Id && expectedPost?.PageHeader?.Title == result?.PageHeader?.Title);
        }

        [Fact]
        public void GetVisiblePostsShouldReturnExpectedSet()
        {
            // Arrange
            var expectedSet = new List<Post>
             {
                new Post
                {
                    Id = 1,
                    PageHeader = new PageHeader { Title = "Test" },
                    User = new ApplicationUser { },
                    PostSections = new List<SectionPost>(),
                },
                new Post
                {
                    Id = 2,
                    PageHeader = new PageHeader { Title = "Test2" },
                    User = new ApplicationUser { },
                    PostSections = new List<SectionPost>(),
                },
                new Post
                {
                    Id = 3,
                    PageHeader = new PageHeader { Title = "Test3" },
                    User = new ApplicationUser { },
                    PostSections = new List<SectionPost>(),
                }, new Post
                {
                    Id = 4,
                    PageHeader = new PageHeader { },
                    User = new ApplicationUser { },
                    PostSections = new List<SectionPost>(),
                }, new Post
                {
                    Id = 5,
                    PageHeader = new PageHeader { },
                    User = new ApplicationUser { },
                    PostSections = new List<SectionPost>(),
                    IsDeleted = false,
                },
             };

            // Act
            var result = this.postService.GetVisiblePosts<PostViewTestModel>().ToList();

            // Assert
            for (int i = 0; i < expectedSet.Count; i++)
            {
                if (result.Count != expectedSet.Count)
                {
                    Assert.True(false);
                }

                Assert.True(expectedSet[i].Id == result[i].Id && expectedSet[i].PageHeader?.Title == result[i].PageHeader?.Title);
            }
        }

        [Fact]
        public async Task OrderByAsyncShouldReturnOrderedSetByPostsSort()
        {
            // Arrange
            PostsSort newest = PostsSort.Newest;
            PostsSort oldest = PostsSort.Oldest;
            PostsSort yours = PostsSort.Yours;

            var inputList = new List<IndexPostViewModel>
            {
                new IndexPostViewModel
                {
                    Id = 1,
                    UserEmail = "dfsdfs",
                    CreatedOn = DateTime.Now.AddHours(-1),
                },
                new IndexPostViewModel
                {
                    Id = 2,
                    UserEmail = "dfsdfs",
                    CreatedOn = DateTime.Now.AddHours(-2),
                },
                new IndexPostViewModel
                {
                    Id = 3,
                    UserEmail = "dfsdfs",
                    CreatedOn = DateTime.Now.AddHours(-3),
                }, new IndexPostViewModel
                {
                    Id = 4,
                    UserEmail = "dfsdfs",
                    CreatedOn = DateTime.Now.AddHours(-4),
                }, new IndexPostViewModel
                {
                    Id = 5,
                    UserEmail = "Pesho",
                    CreatedOn = DateTime.Now.AddHours(-5),
                },
                new IndexPostViewModel
                {
                    Id = 6,
                    UserEmail = "dfsdfs",
                    CreatedOn = DateTime.Now.AddHours(-6),
                },
                new IndexPostViewModel
                {
                    Id = 7,
                    UserEmail = "Pesho",
                    CreatedOn = DateTime.Now.AddHours(-7),
                },
                new IndexPostViewModel
                {
                    Id = 8,
                    UserEmail = "dfsdfs",
                    CreatedOn = DateTime.Now.AddHours(-8),
                }, new IndexPostViewModel
                {
                    Id = 9,
                    UserEmail = "dfsdfs",
                    CreatedOn = DateTime.Now.AddHours(-9),
                }, new IndexPostViewModel
                {
                    Id = 10,
                    UserEmail = "dfsdfs",
                    CreatedOn = DateTime.Now.AddHours(-10),
                },
            };

            var expectedNewestIds = new int[]
            {
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
                9,
                10,
            };
            var expectedOldestIds = expectedNewestIds.OrderByDescending(e => e).ToArray();

            var expectedYoursIds = new int[]
           {
                5,
                7,
           };

            var username = "Pesho";

            // Act
            var newestResult = await this.postService.OrderByAsync(inputList, newest);
            var oldesttResult = await this.postService.OrderByAsync(inputList, oldest);
            var mineResult = await this.postService.OrderByAsync(inputList, yours, username);

            // Assert
            if (newestResult.Count() != expectedNewestIds.Count() || oldesttResult.Count() != expectedOldestIds.Count() || mineResult.Count() != expectedYoursIds.Count())
            {
                Assert.True(false);
            }

            for (int i = 0; i < expectedNewestIds.Length; i++)
            {
                Assert.True(expectedNewestIds[i] == newestResult.ToList()[i].Id);
                Assert.True(expectedOldestIds[i] == oldesttResult.ToList()[i].Id);
            }

            for (int i = 0; i < expectedYoursIds.Length; i++)
            {
                Assert.True(expectedYoursIds[i] == mineResult.ToList()[i].Id);
            }
        }

        [Fact]
        public async Task HidePostByIdAsyncShouldMakeIsDeletedToTrue()
        {
            // Arrange
            int postId = 2;
            var currentPost = this.postsRepository.AllWithDeleted().FirstOrDefault(p => p.Id == postId);

            int unExistingPostId = 999;
            // Act
            await this.postService.HidePostByIdAsync(postId);
            var actualBool = await this.postService.HidePostByIdAsync(unExistingPostId);


            // Assert
            Assert.True(currentPost.IsDeleted == true);
            Assert.False(actualBool);

        }

        [Fact]
        public async Task GetPageHeaderIdAsyncShouldReturnExpectedPageHeaderId()
        {
            // Arrange
            int postId = 2;
            int expectedPageHeaderId = 4;

            // Act
            var actualPageHeaderId = await this.postService.GetPageHeaderIdAsync(postId);

            // Assert
            Assert.True(actualPageHeaderId == expectedPageHeaderId);
        }

        [Fact]
        public async Task GetCreatorsIdAsyncShouldReturnExpectedUserId()
        {
            // Arrange
            int postId = 2;
            string expectedUserId = "sdasdasdasd";

            // Act
            var actualUserId = await this.postService.GetCreatorsIdAsync(postId);

            // Assert
            Assert.True(actualUserId == expectedUserId);
        }

        [Fact]
        public async Task AllHidenPostsShouldReturnAllHiden()
        {
            // Arrange
            var expectedList = new List<PostViewTestModel>
            {
                new PostViewTestModel
                {
                     Id = 6,
                     PageHeader = new PageHeaderTestViewModel { Title = "Test6" },
                },
                new PostViewTestModel
                {
                     Id = 7,
                     PageHeader = new PageHeaderTestViewModel { Title = "dgsdfsdfsg" },
                },
                new PostViewTestModel
                {
                     Id = 8,
                     PageHeader = new PageHeaderTestViewModel { Title = "sdfds" },
                },
                new PostViewTestModel
                {
                     Id = 9,
                     PageHeader = new PageHeaderTestViewModel { Title = "Test9" },
                },
                new PostViewTestModel
                {
                     Id = 10,
                     PageHeader = new PageHeaderTestViewModel { },
                },
            };

            // Act
            var actualSet = await this.postService.AllHidenPosts<PostViewTestModel>();

            // Assert
            if (actualSet.Count != expectedList.Count)
            {
                Assert.True(false);
            }

            for (int i = 0; i < expectedList.Count; i++)
            {
                Assert.True(expectedList[i].Id == actualSet[i].Id);
            }
        }

        [Fact]
        public async Task MakeVisibleAsyncShouldConvertSoftDeletedPostToNotDeleted()
        {
            // Arrange
            var testPost = this.postsRepository.AllWithDeleted().FirstOrDefault(p => p.IsDeleted);
            int notExistingId = 999;
            // Act
            await this.postService.MakeVisibleAsync(testPost.Id);
            var actualRes = await this.postService.MakeVisibleAsync(notExistingId);


            // Assert
            Assert.False(testPost.IsDeleted);
            Assert.False(actualRes);

        }

        private Mock<IDeletableEntityRepository<Post>> GetPostsRepository(List<Post> testPostList)
        {
            var repository = new Mock<IDeletableEntityRepository<Post>>();
            repository.Setup(all => all.All()).Returns(testPostList.Where(x => !x.IsDeleted).AsQueryable().BuildMockDbQuery().Object);
            repository.Setup(all => all.AllWithDeleted()).Returns(testPostList.AsQueryable().BuildMockDbQuery().Object);

            repository.Setup(all => all.AddAsync(It.IsAny<Post>())).Returns(
                (Post post) =>
                {
                    post.Id = testPostList.Count() + 1;
                    testPostList.Add(post);
                    return Task.CompletedTask;
                });

            repository.Setup(all => all.Update(It.IsAny<Post>())).Callback(
                (Post target) =>
                {
                    var post = testPostList.FirstOrDefault(s => s.Id == target.Id);

                    var index = testPostList.IndexOf(post);

                    if (index != -1 || index == 0)
                    {
                        testPostList[index] = post;
                    }
                });

            repository.Setup(all => all.Delete(It.IsAny<Post>())).Callback(
                (Post target) =>
                {
                    var postId = target.Id;
                    testPostList.FirstOrDefault(s => s.Id == postId).IsDeleted = true;
                });

            repository.Setup(all => all.HardDelete(It.IsAny<Post>())).Callback(
                (Post target) =>
                {
                    var postListIndex = testPostList.IndexOf(target);
                    if (postListIndex != 0 || postListIndex != -1)
                    {
                        testPostList.RemoveAt(postListIndex);
                    }
                });

            return repository;
        }

        private List<Post> GetTestPostsList()
        {
            var list = new List<Post>
            {
                new Post
                {
                    Id = 1,
                    PageHeader = new PageHeader { Title = "Test", Id = 1 },
                    User = new ApplicationUser { UserName = "asd", Email = "asd" },
                    PostSections = new List<SectionPost>(),
                    CreatedOn = DateTime.Now.AddHours(-1),
                },
                new Post
                {
                    Id = 2,
                    PageHeader = new PageHeader { Title = "Test2", Id = 4 },
                    User = new ApplicationUser { UserName = "asd", Email = "asd", Id = "sdasdasdasd" },
                    UserId = "sdasdasdasd",
                    PostSections = new List<SectionPost>(),
                    CreatedOn = DateTime.Now.AddHours(-2),
                },
                new Post
                {
                    Id = 3,
                    PageHeader = new PageHeader { Title = "Test3" },
                    User = new ApplicationUser { UserName = "asd", Email = "asd" },
                    PostSections = new List<SectionPost>(),
                    CreatedOn = DateTime.Now.AddHours(-3),
                }, new Post
                {
                    Id = 4,
                    PageHeader = new PageHeader { },
                    User = new ApplicationUser { UserName = "asd", Email = "asd" },
                    PostSections = new List<SectionPost>(),
                    CreatedOn = DateTime.Now.AddHours(-4),
                }, new Post
                {
                    Id = 5,
                    PageHeader = new PageHeader { },
                    User = new ApplicationUser { UserName = "Pesho", Email = "Pesho" },
                    PostSections = new List<SectionPost>(),
                    IsDeleted = false,
                    CreatedOn = DateTime.Now.AddHours(-5),
                },
                new Post
                {
                    Id = 6,
                    PageHeader = new PageHeader { Title = "Test6" },
                    User = new ApplicationUser { UserName = "asd", Email = "asd" },
                    PostSections = new List<SectionPost>(),
                    IsDeleted = true,
                    CreatedOn = DateTime.Now.AddHours(-6),
                },
                new Post
                {
                    Id = 7,
                    PageHeader = new PageHeader { Title = "dgsdfsdfsg" },
                    User = new ApplicationUser { UserName = "Pesho", Email = "Pesho" },
                    PostSections = new List<SectionPost>(),
                    IsDeleted = true,
                    CreatedOn = DateTime.Now.AddHours(-7),
                },
                new Post
                {
                    Id = 8,
                    PageHeader = new PageHeader { Title = "sdfds" },
                    User = new ApplicationUser { UserName = "asd", Email = "asd" },
                    PostSections = new List<SectionPost>(),
                    IsDeleted = true,
                    CreatedOn = DateTime.Now.AddHours(-8),
                }, new Post
                {
                    Id = 9,
                    PageHeader = new PageHeader { Title = "Test9" },
                    User = new ApplicationUser { UserName = "asd", Email = "asd" },
                    PostSections = new List<SectionPost>(),
                    IsDeleted = true,
                    CreatedOn = DateTime.Now.AddHours(-9),
                }, new Post
                {
                    Id = 10,
                    PageHeader = new PageHeader { },
                    User = new ApplicationUser { UserName = "asd", Email = "asd" },
                    PostSections = new List<SectionPost>(),
                    IsDeleted = true,
                    CreatedOn = DateTime.Now.AddHours(-10),
                },
            };
            return list;
        }

        private Mock<IDeletableEntityRepository<ApplicationUser>> GetUserRepository(List<ApplicationUser> testUserList)
        {
            var repository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            repository.Setup(all => all.All()).Returns(testUserList.Where(x => !x.IsDeleted).AsQueryable().BuildMockDbQuery().Object);
            return repository;
        }

        private Mock<IDeletableEntityRepository<Image>> GetImageRepository(List<Image> testImageList)
        {
            var repository = new Mock<IDeletableEntityRepository<Image>>();
            repository.Setup(all => all.All()).Returns(testImageList.Where(x => !x.IsDeleted).AsQueryable().BuildMockDbQuery().Object);
            return repository;
        }

        private List<ApplicationUser> GetTestUsercList()
        {
            var list = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "1",
                    FirstName = "First",
                    LastName = "First",
                    Email = "first@gmail.com",
                    UserName = "first@gmail.com",
                },
                new ApplicationUser
                {
                    Id = "2",
                    FirstName = "Second",
                    LastName = "Second",
                    Email = "second@gmail.com",
                    UserName = "second@gmail.com",
                    Image = new Image
                    {
                        CloudUrl = "cloudUrlTest",
                    },
                },
                new ApplicationUser
                {
                    Id = "3",
                    FirstName = "Third",
                    LastName = "Third",
                    Email = "third@gmail.com",
                    UserName = "third@gmail.com",
                }, new ApplicationUser
                {
                    Id = "4",
                    FirstName = "Fourth",
                    LastName = "Fourth",
                    Image = this.imagesRepository.All().FirstOrDefault(i => i.Id == 1),
                    ImageId = 1,
                    Email = "fourth@gmail.com",
                    UserName = "fourth@gmail.com",
                },
            };

            return list;
        }

        private List<Image> GetTestImagecList()
        {
            var list = new List<Image>
            {
                new Image
                {
                    Id = 1,
                    CloudUrl = "sdfsfsf",
                    ContentType = "sdfsdfsf",
                    ImageTitle = "dsfsdfs",
                },
                new Image
                {
                    Id = 2,
                    CloudUrl = "dgdfgd",
                    ContentType = "sadasd",
                    ImageTitle = "gfhfgh",
                },
                new Image
                {
                    Id = 3,
                    CloudUrl = "retewr",
                    ContentType = "dfsdfdfgh",
                    ImageTitle = "xcvxcvxc",
                }, new Image
                {
                    Id = 4,
                    CloudUrl = "123wfgtrgr",
                    ContentType = "yhrdggjj",
                    ImageTitle = "ewrwefdgb",
                }, new Image
                {
                    Id = 5,
                    CloudUrl = "dddwferhh",
                    ContentType = "fdgdfgerg",
                    ImageTitle = "eqwevdffgn",
                },
            };
            return list;
        }

        private Mock<IDeletableEntityRepository<PageHeader>> GetPageHeadersRepository(List<PageHeader> testPageHeaderList)
        {
            var repository = new Mock<IDeletableEntityRepository<PageHeader>>();
            repository.Setup(all => all.All()).Returns(testPageHeaderList.Where(x => !x.IsDeleted).AsQueryable().BuildMockDbQuery().Object);
            repository.Setup(all => all.AllWithDeleted()).Returns(testPageHeaderList.AsQueryable().BuildMockDbQuery().Object);

            repository.Setup(all => all.AddAsync(It.IsAny<PageHeader>())).Returns(
                (PageHeader pageHeader) =>
                {
                    pageHeader.Id = testPageHeaderList.Count() + 1;
                    testPageHeaderList.Add(pageHeader);
                    return Task.CompletedTask;
                });

            repository.Setup(all => all.Update(It.IsAny<PageHeader>())).Callback(
                (PageHeader target) =>
                {
                    var post = testPageHeaderList.FirstOrDefault(s => s.Id == target.Id);

                    var index = testPageHeaderList.IndexOf(post);

                    if (index != -1 || index == 0)
                    {
                        testPageHeaderList[index] = post;
                    }
                });

            repository.Setup(all => all.Delete(It.IsAny<PageHeader>())).Callback(
                (PageHeader target) =>
                {
                    var postId = target.Id;
                    testPageHeaderList.FirstOrDefault(s => s.Id == postId).IsDeleted = true;
                });

            repository.Setup(all => all.HardDelete(It.IsAny<PageHeader>())).Callback(
                (PageHeader target) =>
                {
                    var postListIndex = testPageHeaderList.IndexOf(target);
                    if (postListIndex != 0 || postListIndex != -1)
                    {
                        testPageHeaderList.RemoveAt(postListIndex);
                    }
                });
            return repository;
        }

        private List<PageHeader> GetTestPageHeadersList()
        {
            var list = new List<PageHeader>
            {
                new PageHeader
                {
                    Id = 1,
                    Image = new Image { },
                    Title = "dsfsd",
                    SubTitle = "qwedfsfs",
                    PageIndicator = "Admin",
                },
                new PageHeader
                {
                    Id = 2,
                    Image = new Image { },
                    Title = "dsfsd",
                    SubTitle = "qwedfsfs",
                    PageIndicator = "SDFSDF",
                },
                new PageHeader
                {
                    Id = 3,
                    Image = new Image { },
                    Title = "dsfsd",
                    SubTitle = "qwedfsfs",
                    PageIndicator = "SDFSDF",
                },
                new PageHeader
                {
                    Id = 4,
                    Image = new Image { },
                    Title = "dsfsd",
                    SubTitle = "qwedfsfs",
                    PageIndicator = "SDFSDF",
                },
                new PageHeader
                {
                    Id = 5,
                    Image = new Image { },
                    Title = "dsfsd",
                    SubTitle = "qwedfsfs",
                    PageIndicator = "SDFSDF",
                },
                new PageHeader
                {
                    Id = 6,
                    Image = new Image { },
                    Title = "dsfsd",
                    SubTitle = "qwedfsfs",
                    PageIndicator = "SDFSDF",
                },
                new PageHeader
                {
                    Id = 7,
                    Image = new Image { },
                    Title = "dsfsd",
                    SubTitle = "qwedfsfs",
                    PageIndicator = "SDFSDF",
                },
            };
            return list;
        }

        private Mock<IDeletableEntityRepository<Section>> GetSectionRepository(List<Section> testSectionList)
        {
            var repository = new Mock<IDeletableEntityRepository<Section>>();
            repository.Setup(all => all.All()).Returns(testSectionList.Where(x => !x.IsDeleted).AsQueryable().BuildMockDbQuery().Object);
            repository.Setup(all => all.AllWithDeleted()).Returns(testSectionList.AsQueryable().BuildMockDbQuery().Object);

            repository.Setup(all => all.AddAsync(It.IsAny<Section>())).Returns(
                (Section section) =>
                {
                    section.Id = testSectionList.Count() + 1;
                    testSectionList.Add(section);
                    return Task.CompletedTask;
                });

            repository.Setup(all => all.Update(It.IsAny<Section>())).Callback(
                (Section target) =>
                {
                    var section = testSectionList.FirstOrDefault(s => s.Id == target.Id);

                    var index = testSectionList.IndexOf(section);

                    if (index != -1 || index == 0)
                    {
                        testSectionList[index] = section;
                    }
                });

            repository.Setup(all => all.Delete(It.IsAny<Section>())).Callback(
                (Section target) =>
                {
                    var sectionId = target.Id;
                    testSectionList.FirstOrDefault(s => s.Id == sectionId).IsDeleted = true;
                });

            repository.Setup(all => all.HardDelete(It.IsAny<Section>())).Callback(
                (Section target) =>
                {
                    var sectionListIndex = testSectionList.IndexOf(target);
                    if (sectionListIndex != 0 || sectionListIndex != -1)
                    {
                        testSectionList.RemoveAt(sectionListIndex);
                    }
                });

            return repository;
        }

        private List<Section> GetTestSectionsList()
        {
            var list = new List<Section>
            {
                new Section
                {
                    Id = 1,
                    Content = "sfsdfgdgdfg",
                    Title = "sdfsdfsdfd",
                    SectionPosts = new List<SectionPost>(),
                },
                new Section
                {
                    Id = 2,
                    Content = "sfsdfgdgdfg",
                    Title = "sdfsdfsdfd",
                    SectionPosts = new List<SectionPost>(),
                },
                new Section
                {
                    Id = 3,
                    Content = "sfsdfgdgdfg",
                    Title = "sdfsdfsdfd",
                    SectionPosts = new List<SectionPost>(),
                }, new Section
                {
                    Id = 4,
                    Content = "sfsdfgdgdfg",
                    Title = "sdfsdfsdfd",
                    SectionPosts = new List<SectionPost>(),
                }, new Section
                {
                    Id = 5,
                    Content = "sfsdfgdgdfg",
                    Title = "sdfsdfsdfd",
                    SectionPosts = new List<SectionPost>(),
                    IsDeleted = false,
                },
                new Section
                {
                    Id = 6,
                    Content = "sfsdfgdgdfg",
                    Title = "sdfsdfsdfd",
                    IsDeleted = true,
                    SectionPosts = new List<SectionPost>(),
                },
                new Section
                {
                    Id = 7,
                    Content = "sfsdfgdgdfg",
                    Title = "sdfsdfsdfd",
                    IsDeleted = true,
                    SectionPosts = new List<SectionPost>(),
                },
                new Section
                {
                    Id = 8,
                    Content = "sfsdfgdgdfg",
                    Title = "sdfsdfsdfd",
                    IsDeleted = true,
                    SectionPosts = new List<SectionPost>(),
                }, new Section
                {
                    Id = 9,
                    Content = "sfsdfgdgdfg",
                    Title = "sdfsdfsdfd",
                    IsDeleted = true,
                    SectionPosts = new List<SectionPost>(),
                }, new Section
                {
                    Id = 10,
                    Content = "sfsdfgdgdfg",
                    Title = "sdfsdfsdfd",
                    IsDeleted = true,
                    SectionPosts = new List<SectionPost>(),
                },
            };
            return list;
        }
    }
}
