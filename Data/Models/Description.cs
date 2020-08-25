using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Data.Enums;

namespace Data.Models
{
    public class Description : BaseModel
    {
        public Language Language { get; set; }
        public string Text { get; set; }
        public Guid ReferenceId { get; set; }
        [ForeignKey("ReferenceId")]
        public Brand Brand { get; set; }
        [ForeignKey("ReferenceId")]
        public Product Product { get; set; }
    }
}
