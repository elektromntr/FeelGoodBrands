using System;
using Xunit;
using Logic.Services;
using Data.Repository;
using Data.Models;
using Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Logic.Services.Interfaces;
using System.Linq;

namespace XUnitTests
{
    public class MediaServiceTests
    {
        DbContextOptions<ApplicationDbContext> _dbContextOptions =
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Guid for unique in-memory database names, to not share dta between tests classes
                .Options;
        
        private IRepository<Media> _mediaRepository;
        private IMediaService _mediaService;

        public MediaServiceTests()
        {
            _mediaRepository =
                new Repository<Media>(new ApplicationDbContext(_dbContextOptions));
            _mediaService = new MediaService(_mediaRepository);
        }

        [Fact]
        public void MediaServiceExists()
        {
            var mediaService = new MediaService(_mediaRepository);
            Assert.NotNull(mediaService);
        }

        [Fact]
        public void CanAddMediaForBrand()
        {
            //Arrange
            var brandGuid = Guid.NewGuid();
            
            //Act
            _mediaService.Add(Data.Enums.MediaType.twitter, string.Empty, brandGuid);
            var testMedia = _mediaRepository.Get()
                .FirstOrDefault(m => m.BrandId.Equals(brandGuid));

            //Assert
            Assert.NotNull(testMedia);
            Assert.Equal(brandGuid, testMedia.BrandId);
        }
    }
}
