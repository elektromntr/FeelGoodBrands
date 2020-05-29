using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
	public abstract class BaseModel
	{
		[Key] public Guid Id { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime? EditDate { get; set; }
	}
}
