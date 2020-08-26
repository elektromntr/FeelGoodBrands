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
    }
}
