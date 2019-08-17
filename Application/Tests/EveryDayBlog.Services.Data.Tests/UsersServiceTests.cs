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
    using EveryDayBlog.Web.Infrastructure.Models;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class UsersServiceTests
    {
        private IDeletableEntityRepository<ApplicationUser> usersRepository;
        private IDeletableEntityRepository<Image> imagesRepository;
        private CloudinaryService cloudinaryService;
        private UsersService userService;

        public UsersServiceTests()
        {
            this.imagesRepository = this.GetImageRepository(this.GetTestImagecList()).Object;
            this.usersRepository = this.GetUserRepository(this.GetTestUsercList()).Object;
            Account cloudinaryAcc
               = new Account(
               "dy78wnfy2",
               "857437556699719",
               "jj3uew_U5wJPCm8Xn_LWYbbHDf8");

            Cloudinary cloudinaryUtility = new Cloudinary(cloudinaryAcc);

            this.cloudinaryService = new CloudinaryService(cloudinaryUtility);

            this.userService = new UsersService(this.usersRepository, this.imagesRepository, this.cloudinaryService);
        }

        [Fact]
        public async Task GetUserByUsernameShouldReturnExpectedUser()
        {
            // Arrange
            var expectedUser = new ApplicationUser
            {
                Id = "1",
                FirstName = "First",
                LastName = "First",
                Email = "first@gmail.com",
                UserName = "first@gmail.com",
            };

            // Act
            var result = await this.userService.GetUserByUsernameAsync("first@gmail.com");

            // Assert
            Assert.Equal(expectedUser.Id, result.Id);
        }

        [Fact]
        public async Task GetUserImageIfExistsShouldReturnExpectedCloudUrl()
        {
            // Arrange
            var expectedFirstUser = new ApplicationUser
            {
                Id = "1",
                FirstName = "First",
                LastName = "First",
                Email = "first@gmail.com",
                UserName = "first@gmail.com",
            };

            var expectedSecondUser = new ApplicationUser
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
            };

            // Act
            var firstResult = await this.userService.GetUserImageIfExistsAsync("first@gmail.com");
            var secondResult = await this.userService.GetUserImageIfExistsAsync("second@gmail.com");

            // Assert
            Assert.Equal(expectedFirstUser.Image?.CloudUrl, firstResult);
            Assert.Equal(expectedSecondUser.Image?.CloudUrl, secondResult);
        }

        [Fact]
        public async Task AddUserImageAsyncShouldAddImageToUser()
        {
            // Arrange
            byte[] img = await File.ReadAllBytesAsync($@"C:\Users\nikolaviktor3132\Pictures\test.png");
            var imgInputModel = new ImageInputModel { CloudUrl = "sdfsdfsdf", ContentType = "sadasd", CreatedOn = DateTime.Now, ImageByte = img, ImageTitle = "weqeqweq", ImagePath = "dsfsdfsdfs" };

            // Act
            var result = await this.userService.AddUserImageAsync(imgInputModel, "third@gmail.com");
            var actualUser = this.usersRepository.All().SingleOrDefault(u => u.UserName == "third@gmail.com");

            // Assert
            Assert.True(actualUser.Image != null);
        }

        [Fact]
        public async Task DeleteUserImgShouldReturnTrue()
        {
            // Arrange
            Image expectedImg = null;
            var testedUser = this.usersRepository.All().FirstOrDefault(u => u.UserName == "fourth@gmail.com");

            // Act
            var result = await this.userService.DeleteUserImgAsync(testedUser.UserName);
            var actualImg = testedUser.Image;

            // Assert
            Assert.True(expectedImg == actualImg);
        }

        [Fact]
        public async Task GetUserByIdShouldReturnExpectedUser()
        {
            // Arrange
            var expectedUser = new ApplicationUser
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
            };

            // Act
            var result = await this.userService.GetUserByIdAsync("2");

            // Assert
            Assert.Equal(expectedUser.Id, result.Id);
        }

        [Fact]
        public async Task GetUserFullNameShouldReturnFullName()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "3",
                FirstName = "Third",
                LastName = "Third",
                Email = "third@gmail.com",
                UserName = "third@gmail.com",
            };
            var expectedFullName = "Third Third";

            // Act
            var result = await this.userService.GetUserFullName(user.UserName);

            // Assert
            Assert.Equal(expectedFullName, result);
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
    }
}
