using Microsoft.AspNetCore.Mvc;
using SportsFacility.Domain.Interface;
using System.Threading.Tasks;

namespace SportsFacility.API.Controllers
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

        public class LoginDto
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class ChangePasswordDto
        {
            public string Email { get; set; } = string.Empty;
            public string CurrentPassword { get; set; } = string.Empty;
            public string NewPassword { get; set; } = string.Empty;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var authResult = await _authService.AuthenticateAsync(login.Email, login.Password);
            
            if (authResult == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new { Token = authResult.Token, Role = authResult.Role });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var result = await _authService.ChangePasswordAsync(dto.Email, dto.CurrentPassword, dto.NewPassword);
            if (!result)
            {
                return BadRequest("Invalid current password or email");
            }

            return Ok(new { Message = "Password changed successfully" });
        }
    }
}
