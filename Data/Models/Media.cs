using Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
	public class Media : BaseModel
	{
		public MediaType Type { get; set; }
		[Url]
		public string Link { get; set; }
		
		[ForeignKey("BrandId")]
		public Brand Brand { get; set; }
		public Guid BrandId { get; set; }
	}
}
