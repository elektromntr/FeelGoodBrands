using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Views.ViewComponents
{
    public class BrandsCarouselViewComponent : ViewComponent
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IRepository<Attachment> _attachmentRepository;
        public BrandsCarouselViewComponent(IRepository<Brand> brandRepository,
            IRepository<Attachment> attachmentRepository)
        {
            _brandRepository = brandRepository;
            _attachmentRepository = attachmentRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _brandRepository.Get().OrderByDescending(b=>b.CreationDate).Take(3).ToList();
            foreach (var brand in model)
            {
                brand.Cover = await _attachmentRepository.GetById(brand.CoverId);
            }
            return View("BrandsCarouselComponent", model);
        }
    }
}
