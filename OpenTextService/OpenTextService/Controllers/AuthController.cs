using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Mvc;
using OpenTextAPI.Responses;

namespace OpenTextAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest form)
        {
            var result = await _authService.LoginAsync(form);
            if (!result.Success)
                return BadRequest(ApiResponse<string>.ErrorResponse(result.ErrorMessage));
            return Ok(ApiResponse<AuthTokenResponse?>.SuccessResponse(result.Data));
        }
    }
}


