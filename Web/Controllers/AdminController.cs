using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Enums;
using Data.Models;
using Data.Repository;
using Logic.DataTransferObjects;
using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IMediaService _mediaService;
        private readonly IRepository<Media> _mediaRepository;
        
        public AdminController(IBrandService brandService,
            IMediaService mediaService,
            IRepository<Media> mediaRepository)
        {
            _brandService = brandService;
            _mediaService = mediaService;
            _mediaRepository = mediaRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateBrand() => View();

        [HttpPost]
        public async Task<IActionResult> CreateBrand(CreateBrandRequest model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);
                var newBrand = await _brandService.Create(model);
                return RedirectToAction("EditBrand", new { id = newBrand.Id });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public async Task<IActionResult> EditBrand(Guid id)
        {
            try
            {
                var brand = await _brandService.GetById(id);
                return View(brand);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditBrand(EditBrandRequest editedBrand)
        {
            if (!ModelState.IsValid) return View(editedBrand);
            var resultBrand = await _brandService.Update(editedBrand);
            return View(resultBrand);
        }

        public async Task<IActionResult> EditBrandLinks(Guid brandGuid)
        {
            var links = await _mediaRepository.Get().Where(m => m.BrandId == brandGuid).ToListAsync();
            return View(links);
        }

        [HttpPost]
        public IActionResult EditBrandLinks(List<Media> media)
        {
            //save links
            return RedirectToAction("EditBrand", new { id = media.FirstOrDefault().BrandId });
        }

        public async Task<IActionResult> AddBrandLink(MediaType mediaType, string link, string brandId)
        {
            var links = await _mediaService.Add(mediaType, link, new Guid(brandId));
            return PartialView("~/Views/Admin/Partials/_BrandLinks.cshtml", links);
        }

        public async Task<IActionResult> DeleteLink(string linkId)
        {
            var links = await _mediaService.Delete(new Guid(linkId));
            return PartialView("~/Views/Admin/Partials/_BrandLinks.cshtml", links);
        }
    }
}
