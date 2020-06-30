using Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Logic.DataTransferObjects
{
    public class EditBrand
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Brak nazwy")]
        public string Name { get; set; }
        public string Description { get; set; }
        public Attachment Cover { get; set; }
        public IFormFile CoverUpdate { get; set; }
        public ICollection<Attachment> Images { get; set; }
        public ICollection<IFormFile> ImagesUpdate { get; set; }
        public ICollection<Media> Medias { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
