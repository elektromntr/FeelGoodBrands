using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Data.Configuration
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{

		}
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Attachment> Attachments { get; set; }
		public DbSet<Media> Medias { get; set; }
		public DbSet<Customer> Customers { get; set; }
	}
}
