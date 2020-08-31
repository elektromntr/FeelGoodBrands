using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Enums;
using Logic.DataTransferObjects;
using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IBrandService brandService,
            IProductService productService,
            IMapper mapper)
        {
            _brandService = brandService;
            _productService = productService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Product/Details/{name}")]
        public async Task<IActionResult> Details(string name)
        {
            var brand = await _productService.GetByName(name);
            return View(_mapper.Map<BrandViewModel>(brand));
        }
    }
}
