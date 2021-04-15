using System.Linq;
using Data.Models;
using Data.Repository;
using Logic.Services;
using Logic.DataTransferObjects;
using Moq;
using Xunit;

namespace XUnitTests
{
    public class BrandServiceTests
    {
        public BrandServiceTests()
        {
        }

        [Fact]
        public void Create_ShouldNotCreateBrandWhenNameAlreadyExist()
        {
            //Arrange
            var testBrand = new CreateBrand(){Name = "Test Brand"};
            var brandRepoMock = new Mock<IRepository<Brand>>();
            brandRepoMock.Setup(m => m
                    .Get()
                    .FirstOrDefault())
                .Returns(new Brand());
            var sut = new BrandService(brandRepoMock.Object, null, null, null, null, null);

            //Act
            var brand = sut.Create(testBrand);

            //Assert
            Assert.NotNull(brand);
        }
    }
}
