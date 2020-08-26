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
    }
}
