using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository;
using Logic.Services.Interfaces;
using System.Linq;

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
    }
}
