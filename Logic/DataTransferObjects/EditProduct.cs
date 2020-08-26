using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Data.Models;
using Microsoft.AspNetCore.Http;

namespace Logic.DataTransferObjects
{
    public class EditProduct
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Brak nazwy")]
        public string Name { get; set; }
        public Attachment Image { get; set; }
        public IFormFile ImageUpdate { get; set; }
        public ICollection<ProductDescription> Descriptions { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public Guid BrandId { get; set; }
    }
}
