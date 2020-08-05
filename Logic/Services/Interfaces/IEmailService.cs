using System.Threading.Tasks;
using Logic.DataTransferObjects;

namespace Logic.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> ContactMe(ContactMeViewModel email);
    }
}
