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
        private readonly IAttachmentService _attachmentService;

        public ProductService(IRepository<Product> productRepository,
            IAttachmentService attachmentService)
        {
            _productRepository = productRepository;
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

        public async Task<Product> GetById(Guid guid) => 
            _productRepository.Get()
                .Where(p => p.Id == guid)
                .Include(p => p.Image)
                .Include(p => p.Descriptions)
                .FirstOrDefault();
    }
}
