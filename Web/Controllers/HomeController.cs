using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Data.Enums;
using Data.Models;
using Data.Repository;
using Logic.DataTransferObjects;
using Logic.Extensions;
using Logic.Services;
using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;

namespace Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IEmailService _emailService;
		private readonly IBrandService _brandService;
		private readonly IRepository<Attachment> _attachmentRepository;

		public HomeController(ILogger<HomeController> logger,
							  IBrandService brandService,
							  IRepository<Attachment> attachmentRepository,
                              IEmailService emailService)
		{
			_logger = logger;
			_brandService = brandService;
			_attachmentRepository = attachmentRepository;
            _emailService = emailService;
        }

		public async Task<IActionResult> Index()
		{

            var brands = await _brandService.GetAll();
            // ViewBag.Path = _brandService.GetLogoUploadPath(); *Brand logo case
			return View(brands);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[HttpPost]
        public JsonResult ContactMe([FromBody] ContactMeViewModel contact)
        {
            _emailService.ContactMe(contact);
            return Json(new {Error = false, contact});
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		public async Task<IActionResult> SeeMore(Guid id)
		{
			var brandGuid = await _brandService.GetBrandByReferenceId(id);
			return RedirectToAction("Details", "Brand", new { id = brandGuid});
		}
	}
}
