using Data.Models;
using Data.Repository;
using Logic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _brandRepository;

        public BrandService(IRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Brand> Create(Brand newBrand)
        {
            var createdBrand = _brandRepository.Add(newBrand);
            await _brandRepository.SaveChangesAsync();
            return await createdBrand;
        }

        public async Task<Brand> GetById(Guid guid) => await _brandRepository.GetById(guid);
    }
}
