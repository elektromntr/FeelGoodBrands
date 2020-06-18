using Microsoft.AspNetCore.Http;

namespace Logic.DataTransferObjects
{
    public class CreateBrandRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Cover { get; set; }
    }
}
