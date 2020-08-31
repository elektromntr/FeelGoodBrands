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
        private readonly IRepository<ProductDescription> _productDescriptionRepository;
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IAttachmentService _attachmentService;

        public ProductService(IRepository<Product> productRepository,
            IRepository<ProductDescription> productDescriptionRepository,
            IRepository<Attachment> attachmentRepository,
            IAttachmentService attachmentService)
        {
            _productRepository = productRepository;
            _productDescriptionRepository = productDescriptionRepository;
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
                .FirstOrDefault();
            result.Image = _attachmentRepository.Get()
                .FirstOrDefault(a => a.ReferenceId == result.Id);
            result.Descriptions = _productDescriptionRepository.Get()
                .Where(d => d.ProductId == result.Id).ToList();
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

        public async Task CreateDescription(ProductDescription description)
        {
            description.CreationDate = DateTime.Now;
            description.EditDate = DateTime.Now;
            await _productDescriptionRepository.Add(description);
            await _productDescriptionRepository.SaveChangesAsync();
        }

        public async Task<ProductDescription> GetDescriptionById(Guid guid) =>
            await _productDescriptionRepository.GetById(guid);

        public void UpdateDescription(ProductDescription description)
        {
            description.EditDate = DateTime.Now;
            _productDescriptionRepository.Update(description);
            _productDescriptionRepository.SaveChanges();
        }

        public async Task<Product> GetByName(string name)
        {
            var guid = _productRepository.Get().FirstOrDefault(b => b.Name.Replace(" ", "") == name).Id;
            var product = await GetById(guid);
            return product;
        }
    }
}
