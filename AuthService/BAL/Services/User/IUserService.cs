using BL.Common;
using BL.Model;

namespace BL.Services.User
{
    public interface IUserService
    {
        Task<Result<UserDto>> RegisterAsync(RegisterRequest request);
        Task<Result<UserDto>> AuthenticateAsync(LoginRequest request);
    }
}
