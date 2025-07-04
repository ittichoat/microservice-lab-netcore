using AuthAPI.Responses;
using BL.Model;
using BL.Services.Token;
using BL.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
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
    }
}
