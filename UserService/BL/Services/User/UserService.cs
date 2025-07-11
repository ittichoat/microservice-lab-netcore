using AutoMapper;
using BL.Common;
using BL.Model;
using DAL.Repository.Interface;

namespace BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterRequest request)
        {
            // 1) ตรวจซ้ำ
            var existingUser = await _repository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                return Result<UserDto>.Fail("Username already exists.");

            // 2) Map และ Hash
            var user = _mapper.Map<Entities.User>(request);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            await _repository.AddUserAsync(user);

            // 3) Map Entity → DTO ส่งออก
            var userDto = _mapper.Map<UserDto>(user);

            return Result<UserDto>.Ok(userDto);
        }

        public async Task<Result<UserDto>> AuthenticateAsync(LoginRequest request)
        {
            var user = await _repository.GetByUsernameAsync(request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Result<UserDto>.Fail("Invalid credentials");

            var userDto = _mapper.Map<UserDto>(user);
            return Result<UserDto>.Ok(userDto);
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<Result<UserDto>> UpdateAsync(int id, UpdateUserRequest request)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return Result<UserDto>.Fail("User not found");

            user.Email = request.Email ?? user.Email;
            user.Username = request.Username ?? user.Username;
            user.PasswordHash = string.IsNullOrEmpty(user.PasswordHash) ? BCrypt.Net.BCrypt.HashPassword(request.Password) : user.PasswordHash;

            await _repository.UpdateUserAsync(user);
            return Result<UserDto>.Ok(_mapper.Map<UserDto>(user));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return false;

            await _repository.DeleteUserAsync(user);
            return true;
        }

    }
}
