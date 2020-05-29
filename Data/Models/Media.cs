using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
	public class Media : BaseModel
	{
		public string Name { get; set; }
		public string Link { get; set; }
		/// <summary>
		/// Media icon
		/// </summary>
		public Guid ImageId { get; set; }
		[NotMapped]
		public Attachment Image { get; set; }
	}
}
