using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Data.Enums;
using Logic.DataTransferObjects;
using Logic.Extensions;
using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public BrandController(IBrandService brandService,
            IMapper mapper,
            IWebHostEnvironment environment)
        {
            _brandService = brandService;
            _mapper = mapper;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        private string GetLogoPath(string brandName)
        {
	        var uploadPath = Path.Combine(_environment.WebRootPath, FileExtension.LogoDirectory);
	        var fileName = brandName.Replace(" ", String.Empty) + ".svg";
	        var filePath = Path.Combine(uploadPath, fileName);
	        return System.IO.File.Exists(filePath) ? filePath : String.Empty;
        }

        [HttpGet]
        [Route("Brand/Details/{name}")]
        public async Task<IActionResult> Details(string name)
        {
            var brand = await _brandService.GetByName(name);
            ViewBag.Path = GetLogoPath(brand.Name);
            return View(_mapper.Map<BrandViewModel>(brand));
        }

        [HttpGet]
        [Route("Brand/Details/{id:Guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
	        try
	        {
		        var brand = await _brandService.GetByIdWithImages(id);
		        ViewBag.Path = GetLogoPath(brand.Name); ;
		        return View(_mapper.Map<BrandViewModel>(brand));
	        }
	        catch (Exception e)
	        {
		        throw new Exception(e.Message);
	        }
        }
    }
}
