using BL.Model;
using BL.Services;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Responses;

namespace UserServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _userService.RegisterAsync(request);

            if (!result.Success)
                return BadRequest(ApiResponse<string>.ErrorResponse("ValidationError", result.ErrorMessage));

            return Ok(ApiResponse<UserDto>.SuccessResponse(result.Data));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _userService.AuthenticateAsync(request);

            if (!result.Success)
                return Unauthorized(ApiResponse<string>.ErrorResponse("Unauthorized", result.ErrorMessage));

            var token = _tokenService.GenerateJwtToken(result.Data!);
            return Ok(ApiResponse<object>.SuccessResponse(new { token }));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();
            return Ok(ApiResponse<List<UserDto>>.SuccessResponse(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound(ApiResponse<string>.ErrorResponse("NotFound", $"User with id {id} not found"));

            return Ok(ApiResponse<UserDto>.SuccessResponse(user));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserRequest request)
        {
            var result = await _userService.UpdateAsync(id, request);

            if (!result.Success)
                return BadRequest(ApiResponse<string>.ErrorResponse("ValidationError", result.ErrorMessage));

            return Ok(ApiResponse<UserDto>.SuccessResponse(result.Data));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _userService.DeleteAsync(id);

            if (!success)
                return NotFound(ApiResponse<string>.ErrorResponse("NotFound", $"User with id {id} not found"));

            return Ok(ApiResponse<string>.SuccessResponse("User deleted successfully"));
        }
    }
}
