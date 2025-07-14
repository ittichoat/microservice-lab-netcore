using Microsoft.AspNetCore.Http;

namespace BL.Models
{
    public class UploadFileRequest
    {
        public IFormFile? File { get; set; }
    }
}
