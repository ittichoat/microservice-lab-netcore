using BL.Common;
using BL.Models;

namespace BL.Services
{
    public interface IAuthService
    {
        Task<Result<AuthTokenResponse?>> LoginAsync(AuthRequest loginRequest);
    }
}
