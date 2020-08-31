using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;

namespace Logic.DataTransferObjects
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }
        public Guid ImageId { get; set; }
        public Attachment Image { get; set; }
        public ICollection<ProductDescription> Descriptions { get; set; }
    }
}
