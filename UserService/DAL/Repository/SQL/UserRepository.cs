using DAL.Models;
using DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using User = DAL.Models.User;

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

        public async Task<List<BL.Entities.User>> GetAllAsync()
        {
            var efUser = await _context.Users.AsNoTracking().Select(s => new BL.Entities.User
            {
                Id = s.Id,
                Username = s.Username,
                Email = s.Email,
                PasswordHash = s.PasswordHash,
                CreatedAt = s.CreatedAt
            }).ToListAsync();
            return efUser;
        }

        public async Task<BL.Entities.User?> GetByIdAsync(int id)
        {
            var efUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
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

        public async Task<bool> UpdateUserAsync(BL.Entities.User user)
        {
            var efUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (efUser == null) return false;

            efUser.Email = user.Email ?? efUser.Email;
            efUser.PasswordHash = string.IsNullOrEmpty(user.PasswordHash) ? efUser.PasswordHash : user.PasswordHash;

            _context.Users.Update(efUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(BL.Entities.User user)
        {
            var efUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (efUser == null) return false;

            _context.Users.Remove(efUser);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}