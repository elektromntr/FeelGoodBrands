using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Views.ViewComponents
{
    public class BrandsCarouselViewComponent : ViewComponent
    {
        private readonly IRepository<Attachment> _attachmentRepository;
        public BrandsCarouselViewComponent(IRepository<Attachment> attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
	        var model = _attachmentRepository.Get()
		        .Where(a => a.InCarousel)
		        .OrderBy(a => a.CarouselOrder).AsEnumerable();
            return View("BrandsCarouselComponent", model);
        }
    }
}
