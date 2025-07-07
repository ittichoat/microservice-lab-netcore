using BL.Entities;

namespace DAL.Repository.Interface
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetByUsernameAsync(string username);
    }
}
