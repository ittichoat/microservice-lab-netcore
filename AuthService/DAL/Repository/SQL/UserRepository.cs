using DAL.Models;
using DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Service
{
    public class UserRepository : IUserRepository
    {
        private readonly user_dbContext _context;

        public UserRepository(user_dbContext context)
        {
            _context = context;
        }

        public Task AddUserAsync(BL.Entities.User user)
        {
            // Mapping ที่นี่: Domain Entity → EF Entity
            var efUser = new User
            {
                Username = user.Username,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                CreatedAt = user.CreatedAt
            };

            _context.Users.Add(efUser);
            return _context.SaveChangesAsync();
        }

        public async Task<BL.Entities.User?> GetByUsernameAsync(string username)
        {
            var efUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
            if (efUser == null) return null;
            return new BL.Entities.User
            {
                Id = efUser.Id,
                Username = efUser.Username,
                Email = efUser.Email,
                PasswordHash = efUser.PasswordHash,
                CreatedAt = efUser.CreatedAt
            };
        }
    }
}