namespace EveryDayBlog.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using EveryDayBlog.Common;
    using EveryDayBlog.Data.Common.Repositories;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Data.Tests.ViewModels;
    using EveryDayBlog.Web.Infrastructure.Models;
    using EveryDayBlog.Web.ViewModels.PageHeaders.InputModels;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class PageHeaderServiceTests : BaseTest
    {
        private IDeletableEntityRepository<PageHeader> pageHeadersRepository;
        private PageHeaderService pageHeaderService;
        private CloudinaryService cloudinaryService;


        public PageHeaderServiceTests()
        {
            Account cloudinaryAcc
               = new Account(
               "dy78wnfy2",
               "857437556699719",
               "jj3uew_U5wJPCm8Xn_LWYbbHDf8");

            Cloudinary cloudinaryUtility = new Cloudinary(cloudinaryAcc);
            this.cloudinaryService = new CloudinaryService(cloudinaryUtility);

            this.pageHeadersRepository = this.GetPageHeadersRepository(this.GetTestPageHeadersList()).Object;
            this.pageHeaderService = new PageHeaderService(this.pageHeadersRepository, this.cloudinaryService);
        }

        [Fact]
        public async Task CreatePageHeaderAsyncShouldAddNewPageHeaderAndReturnExpectedId()
        {
            // Arrange
            var expectedId = 8;

            byte[] img = await File.ReadAllBytesAsync($@"C:\Users\nikolaviktor3132\Pictures\test.png");
            var imgInputModel = new ImageInputModel { CloudUrl = "sdfsdfsdf", ContentType = "sadasd", CreatedOn = DateTime.Now, ImageByte = img, ImageTitle = "weqeqweq", ImagePath = "dsfsdfsdfs" };

            var pageHeaderInputModel = new PageHeaderInputModel
            {
                Image = imgInputModel,
                SubTitle = "dfdfd",
                MainTitle = "dsds",
            };

            // Act
            var actualPageHeader = this.pageHeadersRepository.AllWithDeleted().FirstOrDefault(ph => ph.Id == expectedId);
            Assert.True(actualPageHeader == null);
            await this.pageHeaderService.CreatePageHeaderAsync(pageHeaderInputModel);
            actualPageHeader = this.pageHeadersRepository.AllWithDeleted().FirstOrDefault(ph => ph.Id == expectedId);

            // Assert
            Assert.True(actualPageHeader != null);
        }



       


        [Fact]
        public async Task GetPageHeaderByIdShouldReturnExpectedPageHeader()
        {
            // Arrange
            var inputId = 4;
            var expectedPageHeader = new PageHeader
            {
                Id = 4,
                Image = new Image { },
                Title = "dsfsd",
                SubTitle = "qwedfsfs",
                PageIndicator = "SDFSDF",
            };
            // Act
            PageHeaderViewModel pageHeaderViewModel = await this.pageHeaderService.GetPageHeaderById<PageHeaderViewModel>(inputId);

            // Assert
            Assert.True(pageHeaderViewModel != null);
            Assert.True(pageHeaderViewModel.Id == expectedPageHeader.Id);
            Assert.True(pageHeaderViewModel.Title == expectedPageHeader.Title && pageHeaderViewModel.SubTitle == expectedPageHeader.SubTitle && pageHeaderViewModel.PageIndicator == expectedPageHeader.PageIndicator);
        }

        [Fact]
        public async Task UpdateAsyncShouldReturnExpectedPageHeader()
        {
            // Arrange
            var current = this.pageHeadersRepository.AllWithDeleted().FirstOrDefault();

            var pageHeaderInput = new PageHeaderInputModel { MainTitle = "Update", SubTitle = "Update" };

            // Act
            await this.pageHeaderService.UpdateAsync(current.Id, pageHeaderInput);

            // Assert
            Assert.True(current.Title == pageHeaderInput.MainTitle && current.SubTitle == pageHeaderInput.SubTitle);
        }

        [Fact]
        public async Task GetPageHeadersByPageIndicatorAsyncShouldReturnTheAdminPageHeader()
        {
            // Arrange
            var adminIndicator = GlobalConstants.AdministratorRoleName;
            var randomIndicator = "SDFSDF";

            var expectedFirstSetPageHeader = new List<PageHeader>
            {
                new PageHeader
            {
                Id = 1,
                Image = new Image { },
                Title = "dsfsd",
                SubTitle = "qwedfsfs",
                PageIndicator = GlobalConstants.AdministratorRoleName,
            },
            };

            var expectedSecondSetPageHeader = new List<PageHeader>
            {
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
            // Act

            List<PageHeaderViewModel> firstSetPageHeaderViewModel = await this.pageHeaderService.GetPageHeadersByPageIndicatorAsync<PageHeaderViewModel>(adminIndicator);
            List<PageHeaderViewModel> secondSetPageHeaderViewModel = await this.pageHeaderService.GetPageHeadersByPageIndicatorAsync<PageHeaderViewModel>(randomIndicator);


            // Assert
            Assert.True(firstSetPageHeaderViewModel.Count == expectedFirstSetPageHeader.Count && secondSetPageHeaderViewModel.Count == expectedSecondSetPageHeader.Count);
            Assert.True(expectedFirstSetPageHeader.First().PageIndicator == firstSetPageHeaderViewModel.First().PageIndicator);

            for (int i = 0; i < expectedSecondSetPageHeader.Count; i++)
            {
                Assert.True(expectedSecondSetPageHeader[i].PageIndicator == secondSetPageHeaderViewModel[i].PageIndicator);

            }
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
                    PageIndicator = GlobalConstants.AdministratorRoleName,
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
    }
}
