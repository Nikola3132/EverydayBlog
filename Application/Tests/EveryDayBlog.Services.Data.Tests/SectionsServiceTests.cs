namespace EveryDayBlog.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EveryDayBlog.Data.Common.Repositories;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Services.Data.Tests.ViewModels;
    using EveryDayBlog.Web.ViewModels.Sections.InputModels;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class SectionsServiceTests : BaseTest
    {
        private IDeletableEntityRepository<Section> sectionRepository;
        private SectionService sectionService;

        public SectionsServiceTests()
        {
            this.sectionRepository = this.GetSectionRepository(this.GetTestSectionsList()).Object;
            this.sectionService = new SectionService(this.sectionRepository);
        }

        [Fact]
        public async Task AllDeletedSectionsShouldReturnExpectedSections()
        {
            // Arrange
            var expectedSections = new List<SectionViewTestModel>()
            {
               new SectionViewTestModel
                {
                    Id = 6,
                    Content = "sfsdfgdgdfg",
                    Title = "sdfsdfsdfd",
                    IsDeleted = true,
                },
               new SectionViewTestModel
                {
                    Id = 7,
                    Content = "sfsdfgdgdfg",
                    Title = "sdfsdfsdfd",
                    IsDeleted = true,
                },
               new SectionViewTestModel
                {
                    Id = 8,
                    Content = "sfsdfgdgdfg",
                    Title = "sdfsdfsdfd",
                    IsDeleted = true,
                }, new SectionViewTestModel
                {
                   Id = 9,
                   Content = "sfsdfgdgdfg",
                   Title = "sdfsdfsdfd",
                   IsDeleted = true,
                }, new SectionViewTestModel
                {
                    Id = 10,
                    Content = "sfsdfgdgdfg",
                    Title = "sdfsdfsdfd",
                    IsDeleted = true,
                },
            };

            // Act
            var result = await this.sectionService.AllDeletedSections<SectionViewTestModel>();

            // Assert
            Assert.True(expectedSections.Count == result.Count);
            for (int i = 0; i < expectedSections.Count; i++)
            {
                Assert.True(expectedSections[i].Id == result[i].Id && result[i].IsDeleted);
            }
        }

        [Fact]
        public async Task CreateSectionAsyncShouldReturnExpectedId()
        {
            // Arrange
            var expectedId = 11;
            SectionInputModel expectedSection = new SectionInputModel()
            {
                SectionContent = "dfd",
                SectionTitle = "dfdf",
            };

            // Act
            await this.sectionService.CreateSectionAsync(expectedSection, 1);

            // Assert
            var actualSection = this.sectionRepository.AllWithDeleted().Last();
            Assert.True(expectedSection.SectionContent == actualSection.Content && expectedSection.SectionTitle == actualSection.Title && expectedId == actualSection.Id);
        }

        [Fact]
        public async Task GetSectionByIdAsyncShouldReturnExpectedSection()
        {
            // Arrange
            var expectedSection = new Section
            {
                Id = 5,
                Content = "sfsdfgdgdfg",
                Title = "sdfsdfsdfd",
                SectionPosts = new List<SectionPost>(),
            };

            // Act
            var result = await this.sectionService.GetSectionByIdAsync<SectionViewTestModel>(expectedSection.Id);

            // Assert
            Assert.True(expectedSection.Id == result.Id);
        }

        [Fact]
        public async Task SoftDeleteShouldSetDeletedToTrue()
        {
            // Arrange
            var testedSection = this.sectionRepository.All().Where(s => s.IsDeleted == false).FirstOrDefault();

            // Act
            var result = await this.sectionService.SoftDelete(testedSection.Id);

            // Assert
            Assert.True(testedSection.IsDeleted == true);
        }

        [Fact]
        public async Task HardDeleteShouldSetDeletedToTrue()
        {
            // Arrange
            Section expectedSection = null;
            var testedSection = this.sectionRepository.AllWithDeleted().FirstOrDefault(s => s.Id == 8);

            // Act
            var result = await this.sectionService.HardDelete(testedSection.Id);

            // Assert
            Assert.True(this.sectionRepository.All().FirstOrDefault(s => s.Id == 8) == expectedSection);
        }

        [Fact]
        public async Task ReorganizeAsyncShouldReturnSoftDeletedSectionWithFalseIsDeleted()
        {
            // Arrange
            var testedSection = this.sectionRepository.AllWithDeleted().FirstOrDefault(s => s.IsDeleted == true);

            // Act
            var result = await this.sectionService.ReorganizeAsync(testedSection.Id);

            // Assert
            Assert.True(testedSection.IsDeleted == false);
        }

        [Fact]
        public async Task UpdateSectionByIdAsyncShouldReturnExpectedSection()
        {
            // Arrange
            var expectedSection = new EditSectionInputModel
            {
               Id = 2,
               SectionContent = "Updated",
               SectionTitle = "Updated",
            };

            // Act
            var result = await this.sectionService.UpdateSectionByIdAsync(2, expectedSection);
            var testedSection = this.sectionRepository.AllWithDeleted().FirstOrDefault(s => s.Id == 2);

            // Assert
            Assert.True(expectedSection.SectionContent == testedSection.Content && expectedSection.SectionTitle == expectedSection.SectionTitle && expectedSection.Id == testedSection.Id);
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
