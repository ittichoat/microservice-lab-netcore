using BL.Model;

namespace BL.Services.Token
{
    public interface ITokenService
    {
        string GenerateJwtToken(UserDto user);
    }
}
