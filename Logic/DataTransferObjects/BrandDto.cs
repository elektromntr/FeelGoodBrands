using Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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

    public class EditBrandRequest
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Brak nazwy")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Brak okładki")]
        public IFormFile Cover { get; set; }
        public ICollection<IFormFile> Images { get; set; }
    }
}
