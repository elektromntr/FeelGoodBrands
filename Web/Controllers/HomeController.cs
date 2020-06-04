using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository;
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
			var cover = new Attachment();

			string someUrl = "https://99designs-blog.imgix.net/blog/wp-content/uploads/2018/01/attachment_91140022-e1515601908872.jpeg";
			var webClient = new WebClient();
			
			cover.FileData = webClient.DownloadData(someUrl);
			cover.Id = new Guid();
			cover.CreationDate = DateTime.Now;
			cover.FileMimeType = "image/jpeg";
			var brandGuid = new Guid();
			cover.ReferenceId = brandGuid;

			_attachmentRepository.Add(cover);
			_attachmentRepository.SaveChanges();

			var newBrand = new Brand
			{
				CreationDate = DateTime.Now,
				Description = "Opis testowy",
				Name = $"Brand {new Random().Next(0, 999)}",
				CoverId = cover.Id,
				Id = brandGuid
			};

			var nuBrand = await _brandService.Create(newBrand);

			return View(nuBrand);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
