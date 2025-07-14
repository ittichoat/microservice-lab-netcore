using BL.Common;
using BL.Interfaces;
using BL.Models;

namespace BL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IOpenTextRepository _repo;

        public AuthService(IOpenTextRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<AuthTokenResponse?>> LoginAsync(AuthRequest form)
        {
            var token = await _repo.GetAccessTokenAsync(form);
            if (token == null)
            {
                return Result<AuthTokenResponse?>.Fail("Not found");
            }
            return Result<AuthTokenResponse?>.Ok(token);
        }
    }

}
