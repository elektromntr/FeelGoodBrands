using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository;
using Logic.Services.Interfaces;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Logic.Services
{
	public class AttachmentService : IAttachmentService
	{
		private readonly IRepository<Attachment> _attachmentRepository;

		public AttachmentService(IRepository<Attachment> attachmentRepository)
		{
			_attachmentRepository = attachmentRepository;
		}

		public async Task<List<Attachment>> Delete(Guid attachmentId)
		{
			var attachment = await _attachmentRepository.GetById(attachmentId);
			if (attachment == null) throw new Exception("No Attachment with given Id");
			var referenceId = attachment.ReferenceId;
			_attachmentRepository.Delete(attachment);
			_attachmentRepository.SaveChanges();
			return _attachmentRepository.Get().Where(m => m.ReferenceId == referenceId).ToList();
		}

		public async Task<Guid> SaveCover(Guid referenceGuid, string referenceName, IFormFile cover)
		{
			using (var memoryStream = new MemoryStream())
			{
				await cover.CopyToAsync(memoryStream);
				var coverGuid = Guid.NewGuid();

				// Upload the file if less than 2 MB
				if (memoryStream.Length < 2097152 && cover.ContentType == "image/jpeg")
				{
					var file = new Attachment()
					{
						FileData = memoryStream.ToArray(),
						FileMimeType = cover.ContentType,
						CreationDate = DateTime.Now,
						EditDate = DateTime.Now,
						Id = coverGuid,
						ReferenceId = referenceGuid,
						Description = $"{referenceName} cover image",
						Type = Data.Enums.AttachmentType.Cover
					};

					await _attachmentRepository.Add(file);

					await _attachmentRepository.SaveChangesAsync();
				}
				else
				{
					throw new Exception("File too large or wrong file type (only jpg allowed)");
				}

				return coverGuid;
			}
		}

		public async Task SwitchCarousel(Guid guid)
		{
			var attachmentToCarousel = await _attachmentRepository.GetById(guid);
			attachmentToCarousel.InCarousel = !attachmentToCarousel.InCarousel;
			if (attachmentToCarousel.InCarousel == false)
				attachmentToCarousel.CarouselOrder = 0;
			else
				attachmentToCarousel.CarouselOrder = _attachmentRepository.Get().Max(a => a.CarouselOrder) + 1;
			attachmentToCarousel.EditDate = DateTime.Now;
			await _attachmentRepository.SaveChangesAsync();
		}

		public async Task ChangeCarouselOrder(Guid guid, bool moveUp)
		{
			var thisAttachment = new Attachment();
			var otherAttachment = new Attachment();
			if (moveUp)
			{
				thisAttachment = await _attachmentRepository.GetById(guid);
				otherAttachment = _attachmentRepository.Get().FirstOrDefault(b => b.CarouselOrder == (thisAttachment.CarouselOrder - 1));
				thisAttachment.CarouselOrder -= 1;
				if (otherAttachment != null)
					otherAttachment.CarouselOrder += 1;

			}
			else
			{
				thisAttachment = await _attachmentRepository.GetById(guid);
				otherAttachment = _attachmentRepository.Get().FirstOrDefault(b => b.CarouselOrder == thisAttachment.CarouselOrder + 1);
				thisAttachment.CarouselOrder += 1;
				if (otherAttachment != null)
					otherAttachment.CarouselOrder -= 1;
			}

			thisAttachment.EditDate = DateTime.Now;
			_attachmentRepository.Update(thisAttachment);
			if (otherAttachment != null)
			{
				otherAttachment.EditDate = DateTime.Now;
				_attachmentRepository.Update(otherAttachment);
			}
			await _attachmentRepository.SaveChangesAsync();
		}
	}
}
