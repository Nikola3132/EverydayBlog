namespace EveryDayBlog.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Common.Repositories;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Data.Tests.ViewModels;
    using EveryDayBlog.Web.ViewModels.UsersRequests.InputModels;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class UserRequestServiceTests : BaseTest
    {
        private IDeletableEntityRepository<UserRequest> usersRequestsRepository;
        private UserRequestService userRequestService;

        public UserRequestServiceTests()
        {
            this.usersRequestsRepository = this.GetUserRequestRepository(this.GetTestUserRequestsList()).Object;
            this.userRequestService = new UserRequestService(this.usersRequestsRepository);
        }

        [Fact]
        public async Task SoftDeleteAsyncShouldSetIsDeletedToTrue()
        {
            // Arrange
            // Act
            await this.userRequestService.SoftDeleteAsync(1);
            var currentUr = this.usersRequestsRepository.AllWithDeleted().SingleOrDefault(ur => ur.IsDeleted && ur.Id == 1);

            // Assert
            Assert.True(currentUr?.IsDeleted);
        }

        [Fact]
        public async Task HardDeleteAsyncShouldRemoveTheEntityFromTheSet()
        {
            // Arrange
            var idForDeleting = 6;

            // Act
            await this.userRequestService.HardDeleteAsync(idForDeleting);
            var currentUr = this.usersRequestsRepository.AllWithDeleted().SingleOrDefault(ur => ur.Id == idForDeleting);

            // Assert
            Assert.True(currentUr == null);
        }

        [Fact]
        public async Task MarkAsReadedAsyncShouldSetIsReadedToTrue()
        {
            // Arrange
            // Act
            await this.userRequestService.MarkAsReadedAsync(1);
            var currentUr = this.usersRequestsRepository.AllWithDeleted().SingleOrDefault(ur => ur.IsReaded && ur.Id == 1);

            // Assert
            Assert.True(currentUr?.IsReaded);
        }

        [Fact]
        public async Task SendQuestionAsyncShouldAddNewRequest()
        {
            // Arrange
            var expectedId = 7;
            UserRequestInputModel userRequestInputModel = new UserRequestInputModel
            {
                Email = "dfdfdfd", Message = "Cop Caar", Name = "Mantaaa", Phone = "23232",
            };

            // Act
            await this.userRequestService.SendQuestionAsync(userRequestInputModel);
            var currentUr = this.usersRequestsRepository.All().SingleOrDefault(ur => ur.Id == expectedId);

            // Assert
            Assert.True(currentUr != null);
            Assert.True(currentUr.Id == expectedId);
        }

        [Fact]
        public async Task TakeUserRequestByIdShouldReturnTheExpectedRequest()
        {
            // Arrange
            var expectedId = 6;
            var expectedUserReq = new UserRequest
            {
                Id = 6,
                Message = "dfsfssdasdas",
                Email = "sfsdfdsf@dfdf.cd",
                Name = "fdfdfda",
                Phone = "2323232332",
                IsDeleted = false,
                IsReaded = false,
            };

            // Act
            var res = await this.userRequestService.TakeUserRequestById<UserRequestViewTestModel>(expectedId);

            // Assert
            Assert.True(res != null);
            Assert.True(expectedUserReq.Id == res.Id);
            Assert.True(expectedUserReq.IsReaded == res.IsReaded);
            Assert.True(expectedUserReq.Phone == res.Phone);
            Assert.True(expectedUserReq.Name == res.Name);
            Assert.True(expectedUserReq.Email == res.Email);
            Assert.True(expectedUserReq.Message == res.Message);
        }

        [Fact]
        public async Task TakeAllRequestsShouldReturnAllReqsInTheSet()
        {
            // Arrange
            var expectedSet = new List<UserRequest>
            {
                new UserRequest
                {
                    Id = 1,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = false,
                }, new UserRequest
                {
                    Id = 2,
                    Message = "dfsfssdasdas",
                    Email = "adsf@dfdf.cd",
                    Name = "dgfdfda",
                    Phone = "232332",
                    IsDeleted = false,
                    IsReaded = false,
                }, new UserRequest
                {
                    Id = 3,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = true,
                    IsReaded = true,
                }, new UserRequest
                {
                    Id = 4,
                    Message = "dfsfssdas",
                    Email = "sfsdfdf@dfdf.cd",
                    Name = "fdfdfa",
                    Phone = "2232332",
                    IsDeleted = true,
                    IsReaded = true,
                }, new UserRequest
                {
                    Id = 5,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = false,
                }, new UserRequest
                {
                    Id = 6,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = false,
                }, new UserRequest
                {
                    Id = 7,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = true,
                },
            };

            // Act
            var res = await this.userRequestService.TakeAllRequests<UserRequestViewTestModel>();

            // Assert
            Assert.True(res.Count == expectedSet.Count);

            for (int i = 0; i < expectedSet.Count; i++)
            {
                Assert.True(expectedSet[i].Id == res[i].Id);
            }
        }

        [Fact]
        public async Task TakeAllDeletedRequestsShouldReturnAllSoftDeletedInTheSet()
        {
            // Arrange
            var expectedSet = new List<UserRequest>
            {
                new UserRequest
                {
                    Id = 3,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = true,
                    IsReaded = true,
                }, new UserRequest
                {
                    Id = 4,
                    Message = "dfsfssdas",
                    Email = "sfsdfdf@dfdf.cd",
                    Name = "fdfdfa",
                    Phone = "2232332",
                    IsDeleted = true,
                    IsReaded = true,
                },
            };

            // Act
            var res = await this.userRequestService.TakeAllDeletedRequests<UserRequestViewTestModel>();

            // Assert
            Assert.True(res.Count == expectedSet.Count);

            for (int i = 0; i < expectedSet.Count; i++)
            {
                Assert.True(expectedSet[i].Id == res[i].Id);
            }
        }

        [Fact]
        public async Task TakeAllNonDeletedRequestsShouldReturnAllSoftDeletedInTheSet()
        {
            // Arrange
            var expectedSet = new List<UserRequest>
            {
                new UserRequest
                {
                    Id = 1,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = false,
                }, new UserRequest
                {
                    Id = 2,
                    Message = "dfsfssdasdas",
                    Email = "adsf@dfdf.cd",
                    Name = "dgfdfda",
                    Phone = "232332",
                    IsDeleted = false,
                    IsReaded = false,
                }, new UserRequest
                {
                    Id = 5,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = false,
                }, new UserRequest
                {
                    Id = 6,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = false,
                }, new UserRequest
                {
                    Id = 7,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = true,
                },
            };

            // Act
            var res = await this.userRequestService.TakeAllNonDeletedRequests<UserRequestViewTestModel>();

            // Assert
            Assert.True(res.Count == expectedSet.Count);

            for (int i = 0; i < expectedSet.Count; i++)
            {
                Assert.True(expectedSet[i].Id == res[i].Id);
            }
        }

        [Fact]
        public async Task TakeAllNonReadedRequestsShouldReturnAllNonReadedInTheSet()
        {
            // Arrange
            var expectedSet = new List<UserRequest>
            {
                new UserRequest
                {
                    Id = 1,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = false,
                }, new UserRequest
                {
                    Id = 2,
                    Message = "dfsfssdasdas",
                    Email = "adsf@dfdf.cd",
                    Name = "dgfdfda",
                    Phone = "232332",
                    IsDeleted = false,
                    IsReaded = false,
                }, new UserRequest
                {
                    Id = 5,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = false,
                }, new UserRequest
                {
                    Id = 6,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = false,
                },
            };

            // Act
            var res = await this.userRequestService.TakeAllNonReadedRequests<UserRequestViewTestModel>();

            // Assert
            Assert.True(res.Count == expectedSet.Count);

            for (int i = 0; i < expectedSet.Count; i++)
            {
                Assert.True(expectedSet[i].Id == res[i].Id);
            }
        }

        [Fact]
        public async Task TakeAllReadedRequestsShouldReturnAllReadedInTheSet()
        {
            // Arrange
            var expectedSet = new List<UserRequest>
            {
                new UserRequest
                {
                    Id = 7,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = true,
                },
            };

            // Act
            var res = await this.userRequestService.TakeAllReadedRequests<UserRequestViewTestModel>();

            // Assert
            Assert.True(res.Count == expectedSet.Count);

            for (int i = 0; i < expectedSet.Count; i++)
            {
                Assert.True(expectedSet[i].Id == res[i].Id);
            }
        }

        [Fact]
        public void TakeAllReadedRequestsCountShouldReturnAllExpectedCount()
        {
            // Arrange
            var expectedCount = 1;

            // Act
            var res = this.userRequestService.TakeAllReadedRequestsCount();

            // Assert
            Assert.True(expectedCount == res);
        }

        [Fact]
        public void TakeAllUnReadedRequestsCountShouldReturnAllExpectedCount()
        {
            // Arrange
            var expectedCount = 4;

            // Act
            var res = this.userRequestService.TakeAllUnReadedRequestsCount();

            // Assert
            Assert.True(expectedCount == res);
        }

        [Fact]
        public void TakeAllRequestsCountShouldReturnAllExpectedCount()
        {
            // Arrange
            var expectedCount = 5;

            // Act
            var res = this.userRequestService.TakeAllRequestsCount();

            // Assert
            Assert.True(expectedCount == res);
        }

        [Fact]
        public async Task TakeAllDeletedRequestsShouldReturnExpectedSet()
        {
            // Arrange
            var expectedSet = new List<UserRequest>
            {
               new UserRequest
                {
                    Id = 3,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = true,
                    IsReaded = true,
                }, new UserRequest
                {
                    Id = 4,
                    Message = "dfsfssdas",
                    Email = "sfsdfdf@dfdf.cd",
                    Name = "fdfdfa",
                    Phone = "2232332",
                    IsDeleted = true,
                    IsReaded = true,
                },
            };

            // Act
            var res = await this.userRequestService.TakeAllDeletedRequests<UserRequestViewTestModel>();

            // Assert
            Assert.True(expectedSet.Count == res.Count);

            for (int i = 0; i < expectedSet.Count; i++)
            {
                Assert.True(expectedSet[i].Id == res[i].Id);
            }
        }

        [Fact]
        public async Task AnyDeletedUserRequestsShouldReturnExpectedBool()
        {
            // Arrange

            // Act
            var actual = await this.userRequestService.AnyDeletedUserRequests();

            // Assert
            Assert.True(actual);
        }

        private Mock<IDeletableEntityRepository<UserRequest>> GetUserRequestRepository(List<UserRequest> testUserRequestList)
        {
            var repository = new Mock<IDeletableEntityRepository<UserRequest>>();
            repository.Setup(all => all.All()).Returns(testUserRequestList.Where(x => !x.IsDeleted).AsQueryable().BuildMockDbQuery().Object);
            repository.Setup(all => all.AllWithDeleted()).Returns(testUserRequestList.AsQueryable().BuildMockDbQuery().Object);

            repository.Setup(all => all.AddAsync(It.IsAny<UserRequest>())).Returns(
                (UserRequest userRequest) =>
                {
                    userRequest.Id = testUserRequestList.Count() + 1;
                    testUserRequestList.Add(userRequest);
                    return Task.CompletedTask;
                });

            repository.Setup(all => all.Update(It.IsAny<UserRequest>())).Callback(
                (UserRequest target) =>
                {
                    var post = testUserRequestList.FirstOrDefault(s => s.Id == target.Id);

                    var index = testUserRequestList.IndexOf(post);

                    if (index != -1 || index == 0)
                    {
                        testUserRequestList[index] = post;
                    }
                });

            repository.Setup(all => all.Delete(It.IsAny<UserRequest>())).Callback(
                (UserRequest target) =>
                {
                    var postId = target.Id;
                    testUserRequestList.FirstOrDefault(s => s.Id == postId).IsDeleted = true;
                });

            repository.Setup(all => all.HardDelete(It.IsAny<UserRequest>())).Callback(
                (UserRequest target) =>
                {
                    var postListIndex = testUserRequestList.IndexOf(target);
                    if (postListIndex != 0 || postListIndex != -1)
                    {
                        testUserRequestList.RemoveAt(postListIndex);
                    }
                });
            return repository;
        }

        private List<UserRequest> GetTestUserRequestsList()
        {
            var list = new List<UserRequest>
            {
                new UserRequest
                {
                    Id = 1,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = false,
                }, new UserRequest
                {
                    Id = 2,
                    Message = "dfsfssdasdas",
                    Email = "adsf@dfdf.cd",
                    Name = "dgfdfda",
                    Phone = "232332",
                    IsDeleted = false,
                    IsReaded = false,
                }, new UserRequest
                {
                    Id = 3,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = true,
                    IsReaded = true,
                }, new UserRequest
                {
                    Id = 4,
                    Message = "dfsfssdas",
                    Email = "sfsdfdf@dfdf.cd",
                    Name = "fdfdfa",
                    Phone = "2232332",
                    IsDeleted = true,
                    IsReaded = true,
                }, new UserRequest
                {
                    Id = 5,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = false,
                }, new UserRequest
                {
                    Id = 6,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = false,
                }, new UserRequest
                {
                    Id = 7,
                    Message = "dfsfssdasdas",
                    Email = "sfsdfdsf@dfdf.cd",
                    Name = "fdfdfda",
                    Phone = "2323232332",
                    IsDeleted = false,
                    IsReaded = true,
                },
            };
            return list;
        }
    }
}
