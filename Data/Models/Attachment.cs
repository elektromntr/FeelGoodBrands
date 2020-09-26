using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
	public class Attachment : BaseModel
	{
		public byte[] FileData { get; set; }
		public string FileMimeType { get; set; }
		public Guid ReferenceId { get; set; }
		public string Description { get; set; }
		public AttachmentType Type { get; set; }
		public bool InCarousel { get; set; }
		public int CarouselOrder { get; set; }
	}
}
