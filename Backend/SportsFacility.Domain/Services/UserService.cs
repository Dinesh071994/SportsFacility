using Microsoft.EntityFrameworkCore;
using SportsFacility.Domain.Interface;
using SportsFacility.DTO;
using SportsFacility.Entity.Entities;
using SportsFacility.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> CreateUserAsync(StaffListDto dto)
        {
            var user = new User
            {
                FullName = dto.Name,
                MobileNumber = dto.Phone,
                Email = dto.Email,
                Role = dto.Role,
                IsActive = dto.IsActive,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123")
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserAsync(Guid id, StaffListDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.FullName = dto.Name;
            user.MobileNumber = dto.Phone;
            user.Email = dto.Email;
            user.Role = dto.Role;
            user.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.IsActive = false; // Disable
            user.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResetPasswordAsync(Guid id, string newPassword)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var cleanedEmail = email?.Trim().ToLower() ?? string.Empty;
            return await _context.Users.FirstOrDefaultAsync(u => u.Email != null && u.Email.ToLower() == cleanedEmail);
        }

        public async Task<bool> UpdateProfilePictureAsync(string email, string picturePath)
        {
            var cleanedEmail = email?.Trim().ToLower() ?? string.Empty;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email != null && u.Email.ToLower() == cleanedEmail);
            if (user == null) return false;

            user.ProfilePicture = picturePath;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
