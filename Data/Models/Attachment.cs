using System;

namespace Data.Models
{
	public class Attachment : BaseModel
	{
		public byte[] FileData { get; set; }
		public string FileMimeType { get; set; }
		public Guid ReferenceId { get; set; }
		public string Description { get; set; }
	}
}
