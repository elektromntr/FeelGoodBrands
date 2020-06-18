using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Logic.DataTransferObjects
{
    public class CreateBrandRequest
    {
        [Required(ErrorMessage = "Brak nazwy")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Brak okładki")]
        public IFormFile Cover { get; set; }
    }
}
