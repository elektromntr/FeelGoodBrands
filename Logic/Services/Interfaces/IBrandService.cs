using Data.Models;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.Interfaces
{
    public interface IBrandService
    {
        Task<Brand> Create(Brand newBrand);
    }
}
