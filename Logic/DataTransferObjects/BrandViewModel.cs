using Data.Models;
using System;
using System.Collections.Generic;

namespace Logic.DataTransferObjects
{
    public class BrandViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<BrandDescription> Descriptions { get; set; }
        public Attachment Cover { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Media> Medias { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public int Order { get; set; }
    }
}
