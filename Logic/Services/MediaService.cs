using Data.Enums;
using Data.Models;
using Data.Repository;
using Logic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class MediaService : IMediaService
    {
        private readonly IRepository<Media> _mediaRepository;
        public MediaService(IRepository<Media> mediaRepository)
        {
            _mediaRepository = mediaRepository;
        }
        public async Task<List<Media>> Add(MediaType type, string link, Guid brandId)
        {
            var medias = _mediaRepository.Get().Where(m => m.BrandId == brandId).ToList();
            if (medias.Any(m => m.Link == link)) 
                throw new Exception("Taki link już istnieje");
            await _mediaRepository.Add(new Media
            {
                BrandId = brandId,
                CreationDate = DateTime.Now,
                EditDate = DateTime.Now,
                Link = link,
                Type = type,
                Id = Guid.NewGuid()
            });
            await _mediaRepository.SaveChangesAsync();
            var result = _mediaRepository.Get().Where(m => m.BrandId == brandId).OrderBy(m => m.Type).ToList();
            return result;
        }

        public async Task<List<Media>> Delete(Guid linkId)
        {
            var link = await _mediaRepository.GetById(linkId);
            var brandId = link.BrandId;
            _mediaRepository.Delete(link);
            _mediaRepository.SaveChanges();
            return _mediaRepository.Get().Where(m => m.BrandId == link.BrandId).ToList();
        }
    }
}
