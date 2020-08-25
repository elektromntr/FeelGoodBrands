using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Enums;
using Data.Models;
using Data.Repository;
using Logic.DataTransferObjects;
using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IMediaService _mediaService;
        private readonly IAttachmentService _attachmentService;
        private readonly IRepository<Media> _mediaRepository;
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IMapper _mapper;

        public AdminController(IBrandService brandService,
            IMediaService mediaService,
            IAttachmentService attachmentService,
            IRepository<Media> mediaRepository,
            IRepository<Attachment> attachmentRepository,
            IMapper mapper)
        {
            _brandService = brandService;
            _mediaService = mediaService;
            _attachmentService = attachmentService;
            _mediaRepository = mediaRepository;
            _mapper = mapper;
            _attachmentRepository = attachmentRepository;
        }
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<List<BrandViewModel>>(await _brandService.GetAll()));
        }

        public IActionResult CreateBrand() => View("~/Views/Admin/Brand/CreateBrand.cshtml");

        [HttpPost]
        public async Task<IActionResult> CreateBrand(CreateBrand model)
        {
            try
            {
                if (!ModelState.IsValid) return View("~/Views/Admin/Brand/CreateBrand.cshtml", model);
                var newBrand = await _brandService.Create(model);
                return RedirectToAction("EditBrand", new { id = newBrand.Id });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpGet]
        [Route("Admin/EditBrand/{id:Guid}")]
        public async Task<IActionResult> EditBrand(Guid id)
        {
            try
            {
                var brand = await _brandService.GetByIdWithImages(id);
                var brandToEdit = _mapper.Map<EditBrand>(brand);
                return View("~/Views/Admin/Brand/EditBrand.cshtml", brandToEdit);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditBrand(EditBrand editedBrand)
        {
            if (!ModelState.IsValid) return View(editedBrand);
            var resultBrand = await _brandService.Update(editedBrand);
            var mappedBrand = _mapper.Map<EditBrand>(resultBrand);
            return View(mappedBrand);
        }

        [HttpPost]
        public IActionResult EditBrandLinks(List<Media> media)
        {
            //save links
            return RedirectToAction("EditBrand", new { id = media.FirstOrDefault().BrandId });
        }

        public async Task<IActionResult> AddBrandLink(MediaType mediaType, string link, string brandId)
        {
            var links = await _mediaService.Add(mediaType, link, new Guid(brandId));
            return PartialView("~/Views/Admin/Partials/_BrandLinks.cshtml", links);
        }

        public async Task<IActionResult> DeleteLink(string linkId)
        {
            var links = await _mediaService.Delete(new Guid(linkId));
            return PartialView("~/Views/Admin/Partials/_BrandLinks.cshtml", links);
        }
        
        public async Task<IActionResult> DeletePhoto(string photoId)
        {
            if (photoId == String.Empty) throw new Exception("No photo Id");
            var photos = await _attachmentService.Delete(new Guid(photoId));
            return PartialView("~/Views/Admin/Partials/_BrandPhotos.cshtml", photos);
        }

        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            try
            {
                await _brandService.DeleteBrand(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [HttpGet]
        public IActionResult CreateDescription(Guid guid)
        {
            ViewBag.BrandGuid = guid;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDescription(BrandDescription description)
        {
            await _brandService.CreateDescription(description);
            return RedirectToAction("EditBrand", new {id = description.BrandId});
        }

        [HttpGet]
        public async Task<IActionResult> EditBrandDescription(Guid guid)
        {
            var result = await _brandService.GetDescriptionById(guid);
            return View("~/Views/Admin/Brand/EditBrandDescription.cshtml", result);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditBrandDescription(BrandDescription description)
        {
            _brandService.UpdateDescription(description);
            return RedirectToAction("EditBrand", new {id = description.BrandId});
        }

        
    }
}
