#nullable disable
namespace BL.Models
{
    public class AuthRequest
    {
        public string Region { get; set; } = "us";
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
