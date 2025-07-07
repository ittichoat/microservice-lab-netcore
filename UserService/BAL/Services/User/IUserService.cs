using BL.Common;
using BL.Model;

namespace BL.Services
{
    public interface IUserService
    {
        Task<Result<UserDto>> RegisterAsync(RegisterRequest request);
        Task<Result<UserDto>> AuthenticateAsync(LoginRequest request);
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<Result<UserDto>> UpdateAsync(int id, UpdateUserRequest request);
        Task<bool> DeleteAsync(int id);
    }

}
