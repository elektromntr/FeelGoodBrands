using System;
using System.Threading.Tasks;
using AutoMapper;
using Data.Enums;
using Data.Models;
using Logic.DataTransferObjects;
using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService,
            IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("Product/Details/{name}")]
        public async Task<IActionResult> Details(string name)
        {

            var product = _mapper.Map<ProductViewModel>((await _productService.GetByName(name)));
            return View(product);
        }
    }
}
