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

        public async Task<Brand> Create(CreateBrandRequest newBrand)
        {
            try
            {
                if (_brandRepository.Get().FirstOrDefault(b => b.Name == newBrand.Name) != null) 
                    throw new Exception("Brand o takiej nazwie już istnieje");

                var brandGuid = Guid.NewGuid();
                
                    //using (var memoryStream = new MemoryStream())
                    //{
                    //    await newBrand.Cover.CopyToAsync(memoryStream);

                    //    // Upload the file if less than 2 MB
                    //    if (memoryStream.Length < 2097152 && newBrand.Cover.ContentType == "image/jpeg")
                    //    {
                    //        var file = new Attachment()
                    //        {
                    //            FileData = memoryStream.ToArray(),
                    //            FileMimeType = newBrand.Cover.ContentType,
                    //            CreationDate = DateTime.Now,
                    //            EditDate = DateTime.Now,
                    //            Id = coverGuid,
                    //            ReferenceId = brandGuid,
                    //            Description = $"Cover image for {newBrand.Name} brand",
                    //        };

                    //        await _attachmentRepository.Add(file);

                    //        await _attachmentRepository.SaveChangesAsync();
                    //    }
                    //    else
                    //    {
                    //        throw new Exception("File too large or wrong file type (only jpg allowed)");
                    //    }
                    //}

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

        public async Task<Brand> GetById(Guid guid) => _brandRepository.Get()
            .Where(i => i.Id == guid)
            .Include(b => b.Cover)
            .Include(b => b.Medias)
            .FirstOrDefault();

        public async Task<Brand> Update(EditBrandRequest model)
        {
            var dbBrand = await GetById(model.Id);
            dbBrand.Name = model.Name;
            if (dbBrand.Cover != null)
            {

            }
            return null;
        }
    }
}
