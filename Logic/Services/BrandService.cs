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
using Data.Enums;
using Logic.Extensions;
using MailKit.Search;
using Microsoft.AspNetCore.Hosting;

namespace Logic.Services
{
    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IRepository<Media> _mediaRepository;
        private readonly IRepository<BrandDescription> _brandDescriptionRepository;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _environment;

        public BrandService(IRepository<Brand> brandRepository,
            IRepository<Attachment> attachmentRepository,
            IRepository<Media> mediaRepository,
            IRepository<BrandDescription> brandDescriptionRepository,
            IProductService productService,
            IWebHostEnvironment environment)
        {
            _brandRepository = brandRepository;
            _attachmentRepository = attachmentRepository;
            _mediaRepository = mediaRepository;
            _brandDescriptionRepository = brandDescriptionRepository;
            _productService = productService;
            _environment = environment;
        }

        public async Task<Brand> Create(CreateBrand newBrand)
        {
            try
            {
                if (_brandRepository.Get().FirstOrDefault(b => b.Name == newBrand.Name) != null) 
                    throw new Exception("Brand o takiej nazwie już istnieje");

                var brandGuid = Guid.NewGuid();
                var maxBrandOrder = _brandRepository.Get().MaxAsync(b => b.Order);
                var brandToCreate = new Brand
                {
                    CreationDate = DateTime.Now,
                    EditDate = DateTime.Now,
                    Id = brandGuid,
                    Name = newBrand.Name,
                    Order = await maxBrandOrder +1
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

        private async Task<Guid> SaveCover(Guid brandGuid, string brandName, IFormFile cover)
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
                        Type = Data.Enums.AttachmentType.Cover
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
            foreach (var item in images)
            {
                await using var memoryStream = new MemoryStream();
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
                        Type = Data.Enums.AttachmentType.Regular
                    };

                    await _attachmentRepository.Add(file);
                }
                else
                {
                    throw new Exception("File too large or wrong file type (only jpg allowed)");
                }
            }
            await _attachmentRepository.SaveChangesAsync();
            return true;
        }

        public async Task SaveVectorLogo(IFormFile file, string brandName)
        {
	        if (!file.ContentType.Contains("svg"))
		        throw new Exception("Wrong file format");
	        if (file.Length > 850000)
		        throw new Exception("File exceeded 850 000 file size limit");
	        var uploadPath = Path.Combine(_environment.WebRootPath, FileExtension.LogoDirectory);

	        if (!Directory.Exists(uploadPath))
		        Directory.CreateDirectory(uploadPath);

	        var fileName = brandName + Path.GetExtension(file.FileName);
	        var filePath = Path.Combine(uploadPath, fileName);

	        using (var stream = new FileStream(filePath, FileMode.Create))
	        {
		        await file.CopyToAsync(stream);
	        }
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
                .Include(b => b.Descriptions)
                .Include(p => p.Products)
                    .ThenInclude(p => p.Descriptions)
                .Include(b => b.Products)
                    .ThenInclude(p => p.Image)
                .FirstOrDefault();
            //brand.Images = await _attachmentRepository.Get()
            //    .Where(i => i.ReferenceId == brand.Id 
            //                && i.Type != AttachmentType.Cover).ToListAsync();
            return brand;
        }

        public async Task<Brand> GetByName(string name)
        {
            // ReSharper disable once PossibleNullReferenceException
            var guid = _brandRepository.Get().FirstOrDefault(b => b.Name.Replace(" ", "") == name).Id;
            var brand = await GetByIdWithImages(guid);
            return brand;
        }

        public async Task<Brand> Update(EditBrand model)
        {
            var dbBrand = await GetById(model.Id);
            
            dbBrand.Name = model.Name;
            dbBrand.EditDate = DateTime.Now;
            
            if (model.CoverUpdate != null)
            {
                dbBrand.CoverId = await SaveCover(model.Id, model.Name, model.CoverUpdate);
            }

            if (model.Logo != null)
            {
	            await SaveVectorLogo(model.Logo, model.Name.Replace(" ", String.Empty));
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

        public async Task DeleteBrand(Guid id)
        {
            try
            {
	            var brandToDelete = await _brandRepository.GetById(id);
	            _attachmentRepository.DeleteRange(_attachmentRepository.Get().Where(a => a.ReferenceId == id));
                _mediaRepository.DeleteRange(_mediaRepository.Get().Where(m => m.BrandId == id));
                var brands = _brandRepository.Get().Where(b => b.Order > brandToDelete.Order);
                foreach (var b in brands)
                {
	                b.Order -= 1;
                    _brandRepository.Update(b);
                }
                _brandRepository.Delete(brandToDelete);
                await _brandRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task CreateDescription(BrandDescription description)
        {
            description.CreationDate = DateTime.Now;
            description.EditDate = DateTime.Now;
            _brandDescriptionRepository.Add(description);
            await _brandDescriptionRepository.SaveChangesAsync();
        }

        public async Task<BrandDescription> GetDescriptionById(Guid guid) =>
            await _brandDescriptionRepository.GetById(guid);

        public void UpdateDescription(BrandDescription description)
        {
            description.EditDate = DateTime.Now;
            _brandDescriptionRepository.Update(description);
            _brandDescriptionRepository.SaveChanges();
        }

        public async Task ChangeBrandOrder(Guid brandGuid, bool moveUp)
        {
            var brand = new Brand();
            var otherBrand = new Brand();
	        if (moveUp)
	        {
		        brand = await _brandRepository.GetById(brandGuid);
		        otherBrand = _brandRepository.Get().First(b => b.Order == (brand.Order - 1));
		        brand.Order -= 1;
		        otherBrand.Order += 1;
                
	        }
	        else
	        {
		        brand = await _brandRepository.GetById(brandGuid);
		        otherBrand = _brandRepository.Get().First(b => b.Order == brand.Order + 1);
		        brand.Order += 1;
		        otherBrand.Order -= 1;
            }
	        brand.EditDate = DateTime.Now;
	        otherBrand.EditDate = DateTime.Now;
	        _brandRepository.Update(brand);
	        _brandRepository.Update(otherBrand);
            _brandRepository.SaveChanges();
        }

        public async Task<Guid> GetBrandByReferenceId(Guid referenceId)
        {
            var brand = (await _brandRepository.GetById(referenceId))?.Id;
            var prod = await _productService.GetBrandIdForProduct(referenceId);
            if (brand != null && brand != Guid.Empty)
	            return brand.GetValueOrDefault();
            else
	            return prod;
        }

        public async Task<List<Brand>> GetAll() => await 
            _brandRepository.Get()
                .Include(b => b.Descriptions)
                .Include(b => b.Cover)
                .Where(b => b.CoverId != null 
                            && b.CoverId != System.Guid.Empty
                            && !b.Archived)
                .OrderBy(b => b.Order)
                .ToListAsync();
        
        public async Task<List<Attachment>> GetAttachmentsFromActiveBrands()
        {
	        var brands = _brandRepository.Get()
		        .Include(b => b.Cover)
		        .Include(b => b.Products).ThenInclude(p => p.Image)
		        .AsNoTracking()
		        .Where(b => !b.Archived)
		        .OrderBy(a => a.Order)
		        .AsEnumerable();

	        var atts = new List<Attachment>();

	        foreach (var b in brands)
	        {
		        atts.Add(b.Cover);
                atts.AddRange(b.Products.Select(c => c.Image));
	        }

	        return atts.ToList();
        }
    }
}
