using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;

namespace Data.Models
{
	public class Brand : BaseModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public ICollection<Attachment> Images { get; set; }
		/// <summary>
		/// Urls for brand
		/// </summary>
		public ICollection<Media> Medias { get; set; }
		public Guid CoverId { get; set; }
		[ForeignKey("CoverId")]
		public Attachment Cover { get; set; }
	}
}
