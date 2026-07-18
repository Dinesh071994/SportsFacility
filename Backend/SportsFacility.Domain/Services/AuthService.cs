using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SportsFacility.Domain.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using SportsFacility.DTO;
using SportsFacility.Domain.Infra;

namespace SportsFacility.Domain.Services
{
    // Services/AuthService.cs
    public class AuthService
    {
        private readonly SportsFacilityDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(SportsFacilityDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> Authenticate(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            var token = GenerateJwtToken(user);
            return token;
        }

        public async Task<string> Register(RegisterUserDto userDto)
        {
            var user = new User
            {
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PhoneNumber = userDto.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            return token;
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("role", user.Role)
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
