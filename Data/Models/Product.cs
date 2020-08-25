using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Product : BaseModel
    {
        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }
        public Guid BrandId { get; set; }
        [ForeignKey("ImageId")]
        public Attachment Image { get; set; }
        public Guid ImageId { get; set; }
        public ICollection<Description> Descriptions { get; set; }
    }
}