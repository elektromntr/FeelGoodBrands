using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository;
using Logic.DataTransferObjects;
using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;

namespace Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IBrandService _brandService;
		private readonly IRepository<Attachment> _attachmentRepository;

		public HomeController(ILogger<HomeController> logger,
							  IBrandService brandService,
							  IRepository<Attachment> attachmentRepository)
		{
			_logger = logger;
			_brandService = brandService;
			_attachmentRepository = attachmentRepository;
		}

		public async Task<IActionResult> Index()
        {
            var brands = await _brandService.GetAll();
			return View(brands);
		}

		public IActionResult Privacy()
		{
			return View();
		}

        public JsonResult ContactMe([FromBody] ContactMeViewModel contact)
        {
            return Json(new {Error = false, contact});
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
