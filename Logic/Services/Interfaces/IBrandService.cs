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
        Task<Brand> Create(CreateBrandRequest newBrand);
        Task<Brand> GetById(Guid guid);
    }
}
