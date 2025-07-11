using BL.Entities;

namespace DAL.Repository.Interface
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetByUsernameAsync(string username);
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(User user);
    }
}
