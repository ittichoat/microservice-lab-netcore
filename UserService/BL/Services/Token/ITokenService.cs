using BL.Model;

namespace BL.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(UserDto user);
    }
}
