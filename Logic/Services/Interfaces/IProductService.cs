using System;
using System.Threading.Tasks;
using Data.Models;
using Logic.DataTransferObjects;

namespace Logic.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> Create(CreateProduct product);
        Task<Product> GetById(Guid guid);
        Task<Product> Update(EditProduct model);
        Task CreateDescription(ProductDescription description);
        Task<ProductDescription> GetDescriptionById(Guid guid);
        void UpdateDescription(ProductDescription description);
        Task<Product> GetByName(string name);
    }
}
