using Data.Models;
using Data.Repository;
using Logic.DataTransferObjects;
using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Logic.Services
{
    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IRepository<Attachment> _attachmentRepository;

        public BrandService(IRepository<Brand> brandRepository,
            IRepository<Attachment> attachmentRepository)
        {
            _brandRepository = brandRepository;
            _attachmentRepository = attachmentRepository;
        }

        public async Task<Brand> Create(CreateBrand newBrand)
        {
            try
            {
                if (_brandRepository.Get().FirstOrDefault(b => b.Name == newBrand.Name) != null) 
                    throw new Exception("Brand o takiej nazwie już istnieje");

                var brandGuid = Guid.NewGuid();
                
                var brandToCreate = new Brand
                {
                    CreationDate = DateTime.Now,
                    Description = newBrand.Description,
                    EditDate = DateTime.Now,
                    Id = brandGuid,
                    Name = newBrand.Name
                };

                if (newBrand.Cover == null) throw new Exception("Brak zdjęcia głównego dla brandu");
                brandToCreate.CoverId = await SaveCover(brandGuid, newBrand.Name, newBrand.Cover);

                var createdBrand = _brandRepository.Add(brandToCreate);
                await _brandRepository.SaveChangesAsync();
                return await createdBrand;
            }
            catch (Exception e)
            {
                throw new Exception("Brand hasn't been created", e);
            }
        }

        private async Task<Guid> SaveCover (Guid brandGuid, string brandName, IFormFile cover)
        {
            using (var memoryStream = new MemoryStream())
            {
                await cover.CopyToAsync(memoryStream);
                var coverGuid = Guid.NewGuid();

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152 && cover.ContentType == "image/jpeg")
                {
                    var file = new Attachment()
                    {
                        FileData = memoryStream.ToArray(),
                        FileMimeType = cover.ContentType,
                        CreationDate = DateTime.Now,
                        EditDate = DateTime.Now,
                        Id = coverGuid,
                        ReferenceId = brandGuid,
                        Description = $"Cover image for {brandName} brand",
                    };

                    await _attachmentRepository.Add(file);

                    await _attachmentRepository.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("File too large or wrong file type (only jpg allowed)");
                }
                return coverGuid;
            }
        }

        private async Task<bool> SaveImages(Guid brandGuid, string brandName, ICollection<IFormFile> images)
        {
            using (var memoryStream = new MemoryStream())
            {
                foreach (var item in images)
                {
                    await item.CopyToAsync(memoryStream);
                    var imageGuid = Guid.NewGuid();

                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 2097152 && item.ContentType == "image/jpeg")
                    {
                        var file = new Attachment()
                        {
                            FileData = memoryStream.ToArray(),
                            FileMimeType = item.ContentType,
                            CreationDate = DateTime.Now,
                            EditDate = DateTime.Now,
                            Id = imageGuid,
                            ReferenceId = brandGuid,
                            Description = $"Image for {brandName} brand",
                        };

                        await _attachmentRepository.Add(file);
                    }
                    else
                    {
                        throw new Exception("File too large or wrong file type (only jpg allowed)");
                    }
                } 
                        await _attachmentRepository.SaveChangesAsync();
            }
            return true;
        }

        public async Task<Brand> GetById(Guid guid) => _brandRepository.Get()
            .Where(i => i.Id == guid)
            .Include(b => b.Cover)
            .Include(b => b.Medias)
            .FirstOrDefault();

        public async Task<Brand> GetByIdWithImages(Guid guid)
        {
            var brand = _brandRepository.Get()
            .Where(i => i.Id == guid)
            .Include(b => b.Cover)
            .Include(b => b.Medias)
            .FirstOrDefault();
            brand.Images = await _attachmentRepository.Get().Where(i => i.ReferenceId == brand.Id).ToListAsync();
            return brand;
        }

        public async Task<Brand> Update(EditBrand model)
        {
            var dbBrand = await GetById(model.Id);
            dbBrand.Name = model.Name;
            if (model.CoverUpdate != null)
            {
                dbBrand.CoverId = await SaveCover(model.Id, model.Name, model.CoverUpdate);
            }
            _brandRepository.Update(dbBrand);
            _brandRepository.SaveChanges();
            bool imagesUpdate;
            if (model.ImagesUpdate != null)
            {
                imagesUpdate = await SaveImages(model.Id, model.Name, model.ImagesUpdate);
            }
            return await GetByIdWithImages(model.Id);
        }
    }
}
