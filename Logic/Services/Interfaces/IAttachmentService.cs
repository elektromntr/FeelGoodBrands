using Data.Enums;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.Interfaces
{
    public interface IAttachmentService
    {
        Task<List<Attachment>> Delete(Guid attachmentId);
    }
}
