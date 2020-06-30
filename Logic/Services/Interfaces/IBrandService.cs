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
    }
}
