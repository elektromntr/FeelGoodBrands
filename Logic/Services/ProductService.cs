using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository;
using Logic.DataTransferObjects;
using Logic.Services.Interfaces;

namespace Logic.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IAttachmentService _attachmentService;

        public ProductService(IRepository<Product> productRepository,
            IRepository<Attachment> attachmentRepository,
        IAttachmentService attachmentService)
        {
            _productRepository = productRepository;
            _attachmentRepository = attachmentRepository;
            _attachmentService = attachmentService;
        }

        public async Task<Product> Create(CreateProduct product)
        {
            var productGuid = Guid.NewGuid();

            var productToCreate = new Product
            {
                CreationDate = DateTime.Now,
                EditDate = DateTime.Now,
                Id = productGuid,
                Name = product.Name,
                BrandId = product.BrandId
            };

            if (product.Image == null) throw new Exception("Brak zdjęcia głównego dla produktu");
            productToCreate.ImageId = await _attachmentService.SaveCover(productGuid, productToCreate.Name, product.Image);

            var result = _productRepository.Add(productToCreate);
            await _productRepository.SaveChangesAsync();
            return await result;
        }

        public async Task<Product> GetById(Guid guid)
        {
            var result = _productRepository.Get()
                .Where(p => p.Id == guid)
                .Include(p => p.Image)
                .Include(p => p.Descriptions)
                .FirstOrDefault();
            result.Image = _attachmentRepository.Get()
                .FirstOrDefault(a => a.ReferenceId == result.Id);
            return result;
        }

        public async Task<Product> Update(EditProduct model)
        {
            var dbProduct = await GetById(model.Id);

            dbProduct.Name = model.Name;
            dbProduct.EditDate = DateTime.Now;

            if (model.ImageUpdate != null)
                dbProduct.ImageId = await _attachmentService.SaveCover(model.Id, model.Name, model.ImageUpdate);
            
            _productRepository.Update(dbProduct);
            _productRepository.SaveChanges();
            
            return await GetById(model.Id);
        }
    }
}
