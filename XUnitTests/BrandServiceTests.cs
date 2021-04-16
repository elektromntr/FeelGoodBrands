using Data.Models;
using Data.Repository;
using Logic.Services;
using Logic.DataTransferObjects;
using Moq;
using Xunit;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Logic.Extensions;

namespace XUnitTests
{
    public class BrandServiceTests
    {
        private Mock<IRepository<Brand>> _brandRepoMock;
        private Guid _testGuid;

        public BrandServiceTests()
        {
            _brandRepoMock = new Mock<IRepository<Brand>>();
            _testGuid = Guid.NewGuid();
        }

        [Fact]
        public async Task Create_ShouldNotCreateBrandWithoutCoverPhoto()
		{
            //Arrange
            var testBrand = new CreateBrand { Name = "Brand without cover" };

            _brandRepoMock.Setup(m => m
                    .GetByExpression(It.IsAny<Expression<Func<Brand, bool>>>()))
                .Returns(new List<Brand>());
            var sut = new BrandService(_brandRepoMock.Object, null, null, null, null, null);

            //Act
            //Assert
            await Assert.ThrowsAsync<Exception>(async () => await sut.Create(testBrand));
        }

        [Fact]
        public async Task Create_ShouldNotCreateBrandWhenNameAlreadyExist()
        {
            //Arrange
            var testBrand = new CreateBrand {Name = "Test Brand"};
            
            _brandRepoMock.Setup(m => m
                    .GetByExpression(It.IsAny<Expression<Func<Brand, bool>>>()))
                .Returns(BrandsList());
            var sut = new BrandService(_brandRepoMock.Object, null, null, null, null, null);

            //Act
            //Assert
            await Assert.ThrowsAsync<Exception>(async () => await sut.Create(testBrand));
        }

        [Fact]
        public async Task GetByName_ReturnsBrandWithSameName()
		{
            //Arrange
            var testBrand = new CreateBrand { Name = "Test name brand" };
            _brandRepoMock.Setup(m => m
                    .GetByExpression(It.IsAny<Expression<Func<Brand, bool>>>()))
                .Returns(BrandsList());
            _brandRepoMock.Setup(m => m
                    .Get())
                .Returns(BrandsList().AsQueryable());
            var sut = new BrandService(_brandRepoMock.Object, null, null, null, null, null);

            //Act
            var brand = await sut.GetByName("Test name brand");

            //Assert
            Assert.NotNull(brand);
            Assert.Equal(testBrand.Name.ToComparable(), brand.Name.ToComparable());
        }

        [Fact]
        public async Task GetById_ReturnsOneElementWithSameGuid()
		{
            //Arrange
            _brandRepoMock.Setup(m => m
                    .Get())
                .Returns(BrandsList().AsQueryable());
            var sut = new BrandService(_brandRepoMock.Object, null, null, null, null, null);

            //Act
            var brand = await sut.GetById(_testGuid);

            //Assert
            Assert.NotNull(brand);
            Assert.Equal(_testGuid, brand.Id);
        }

        IEnumerable<Brand> BrandsList()
        {
            return new List<Brand>
            {
                new Brand
                {
                    Archived = false,
                    Name = "Test brand",
                    Order = 1
                },
                new Brand
                {
                    Id = _testGuid,
                    Archived = false,
                    Name = "Test name brand",
                    Order = 2
                }
            };
        }
    }
}
