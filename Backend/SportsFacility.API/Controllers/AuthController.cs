using Microsoft.AspNetCore.Mvc;
using SportsFacility.Domain.Interface;
using System.Threading.Tasks;

namespace SportsFacility.API.Controllers
{
    public class AuthController : BaseApiController
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

        public class RefreshTokenDto
        {
            public string Token { get; set; } = string.Empty;
            public string RefreshToken { get; set; } = string.Empty;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var authResult = await _authService.AuthenticateAsync(login.Email, login.Password);
            
            if (authResult == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new { Token = authResult.Token, RefreshToken = authResult.RefreshToken, Role = authResult.Role });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
        {
            var authResult = await _authService.RefreshTokenAsync(dto.Token, dto.RefreshToken);
            if (authResult == null)
            {
                return BadRequest("Invalid token or refresh token");
            }
            return Ok(new { Token = authResult.Token, RefreshToken = authResult.RefreshToken, Role = authResult.Role });
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
