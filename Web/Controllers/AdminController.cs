using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRepository<Brand> _brandRepository;
        
        public AdminController(IRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateBrand() => View();
    }
}
