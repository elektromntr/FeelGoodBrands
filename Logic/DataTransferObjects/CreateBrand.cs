using Data.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Logic.DataTransferObjects
{
    public class CreateBrand
    {
        [Required(ErrorMessage = "Brak nazwy")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Brak okładki")]
        public IFormFile Cover { get; set; }
    }
}
