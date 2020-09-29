using Data.Enums;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Logic.Services.Interfaces
{
    public interface IAttachmentService
    {
        Task<List<Attachment>> Delete(Guid attachmentId);
        Task<Guid> SaveCover(Guid referenceGuid, string referenceName, IFormFile cover);
        Task SwitchCarousel(Guid guid);
        Task ChangeCarouselOrder(Guid guid, bool moveUp);
    }
}
