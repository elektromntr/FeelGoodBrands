using Data.Models;
using Data.Repository;
using Logic.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.Interfaces
{
    public interface IBrandService
    {
        Task<Brand> Create(CreateBrand newBrand);
        Task<Brand> GetById(Guid guid);
        Task<Brand> Update(EditBrand model);
        Task<Brand> GetByIdWithImages(Guid guid);
        Task<Brand> GetByName(string name);
        Task<List<Brand>> GetAll();
        Task DeleteBrand(Guid id);
        Task CreateDescription(BrandDescription description);
        Task<BrandDescription> GetDescriptionById(Guid guid);
        void UpdateDescription(BrandDescription description);
        Task ChangeBrandOrder(Guid brandGuid, bool moveUp);
        Task<Guid> GetBrandByReferenceId(Guid referenceId);
    }
}
