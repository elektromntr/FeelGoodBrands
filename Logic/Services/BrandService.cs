using Data.Models;
using Data.Repository;
using Logic.DataTransferObjects;
using Logic.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Web.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

                var coverGuid = Guid.NewGuid();
                var brandGuid = Guid.NewGuid();

                if (newBrand.Cover != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await newBrand.Cover.CopyToAsync(memoryStream);

                        // Upload the file if less than 2 MB
                        if (memoryStream.Length < 2097152 && newBrand.Cover.ContentType == "image/jpeg")
                        {
                            var file = new Attachment()
                            {
                                FileData = memoryStream.ToArray(),
                                FileMimeType = newBrand.Cover.ContentType,
                                CreationDate = DateTime.Now,
                                EditDate = DateTime.Now,
                                Id = coverGuid,
                                ReferenceId = brandGuid,
                                Description = $"Cover image for {newBrand.Name} brand",
                            };

                            _attachmentRepository.Add(file);

                            await _attachmentRepository.SaveChangesAsync();
                        }
                        else
                        {
                            throw new Exception("File too large");
                        }
                    }
                }

                var brandToCreate = new Brand
                {
                    CoverId = coverGuid,
                    CreationDate = DateTime.Now,
                    Description = newBrand.Description,
                    EditDate = DateTime.Now,
                    Id = brandGuid,
                    Name = newBrand.Name
                };

                var createdBrand = _brandRepository.Add(brandToCreate);
                await _brandRepository.SaveChangesAsync();
                return await createdBrand;
            }
            catch (Exception e)
            {
                throw new Exception("Brand hasn't been created", e);
            }
        }

        public async Task<Brand> GetById(Guid guid) => await _brandRepository.GetById(guid);
    }
}
