using SportsFacility.Entity.Entities;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Interface
{
    public class AuthResult
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public interface IAuthService
    {
        Task<AuthResult?> AuthenticateAsync(string email, string password);
        Task<AuthResult?> RefreshTokenAsync(string token, string refreshToken);
        Task<bool> RegisterAsync(string fullName, string email, string mobileNumber, string password, string role);
        Task<bool> ChangePasswordAsync(string email, string currentPassword, string newPassword);
    }
}
