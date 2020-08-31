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
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;

        public BrandController(IBrandService brandService,
            IMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Brand/Details/{name}")]
        public async Task<IActionResult> Details(string name)
        {
            var brand = await _brandService.GetByName(name);
            return View(_mapper.Map<BrandViewModel>(brand));
        }
    }
}
