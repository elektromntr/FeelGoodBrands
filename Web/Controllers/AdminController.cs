using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository;
using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IBrandService _brandService;
        
        public AdminController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateBrand() => View();

        public async Task<IActionResult> AddBrand(Brand model)
        {
            var newBrand = await _brandService.Create(model);
            return RedirectToAction("EditBrand", new { id = newBrand.Id });
        }

        public async Task<IActionResult> EditBrand(Guid id)
        {
            var brand = await _brandService.GetById(id);
            return View(brand);
        }
    }
}
