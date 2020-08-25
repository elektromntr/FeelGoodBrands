using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Data.Enums;

namespace Data.Models
{
    public class BrandDescription : BaseModel
    {
        public Language Language { get; set; }
        public string Text { get; set; }
        public Guid BrandId { get; set; }
        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }
    }
}
