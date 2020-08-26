using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Logic.DataTransferObjects
{
    public class CreateProduct
    {
        [Required(ErrorMessage = "Brak nazwy")]
        public string Name { get; set; }
        public Guid BrandId { get; set; }
        [Required(ErrorMessage = "Brak zdjęcia")]
        public IFormFile Image { get; set; }
    }
}
