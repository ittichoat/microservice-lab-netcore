using AutoMapper;
using BL.Common;
using BL.Model;
using DAL.Repository.Interface;

namespace BL.Services.User
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
    }
}
