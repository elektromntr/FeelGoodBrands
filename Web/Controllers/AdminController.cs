using System;
using System.Linq;
using System.Threading.Tasks;
using Logic.DataTransferObjects;
using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
    }
}
