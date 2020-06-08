using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Views.ViewComponents
{
    public class BrandsCarouselViewComponent : ViewComponent
    {
        private readonly IRepository<Brand> _brandRepository;
        public BrandsCarouselViewComponent(IRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _brandRepository.Get().OrderByDescending(b=>b.EditDate).Take(3);

            return View("BrandsCarouselComponent", model);
        }
    }
}
