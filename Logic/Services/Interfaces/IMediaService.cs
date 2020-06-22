using Data.Enums;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.Interfaces
{
    public interface IMediaService
    {
        Task<List<Media>> Add(MediaType type, string link, Guid brandId);
        Task<List<Media>> Delete(Guid linkId);
    }
}
