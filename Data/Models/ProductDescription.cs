using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Data.Enums;

namespace Data.Models
{
    public class ProductDescription : BaseModel
    {
        public Language Language { get; set; }
        public string Text { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Brand { get; set; }
    }
}
