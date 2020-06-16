using Data.Models;
using Data.Repository;
using Logic.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly ILogger _logger;

        public BrandService(IRepository<Brand> brandRepository,
            ILogger logger)
        {
            _brandRepository = brandRepository;
            _logger = logger;
        }

        public async Task<Brand> Create(Brand newBrand)
        {
            try
            {
                if (_brandRepository.Get().FirstOrDefault(b => b.Name == newBrand.Name) != null) 
                    throw new Exception("Brand o takiej nazwie już istnieje");
                var createdBrand = _brandRepository.Add(newBrand);
                await _brandRepository.SaveChangesAsync();
                return await createdBrand;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<Brand> GetById(Guid guid) => await _brandRepository.GetById(guid);
    }
}
