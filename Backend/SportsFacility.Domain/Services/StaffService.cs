using Microsoft.EntityFrameworkCore;
using SportsFacility.Domain.Interface;
using SportsFacility.DTO;
using SportsFacility.Entity.Entities;
using SportsFacility.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Services
{
    public class StaffService : IStaffService
    {
        private readonly ApplicationDbContext _context;

        public StaffService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetStaffAsync()
        {
            return await _context.Users
                .Where(u => u.Role != "Member" && u.Role != "Customer")
                .ToListAsync();
        }

        public async Task<User?> CreateStaffAsync(StaffListDto dto)
        {
            var user = new User
            {
                FullName = dto.Name,
                MobileNumber = dto.Phone,
                Email = dto.Email,
                Role = dto.Role,
                IsActive = dto.IsActive,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Staff@123")
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateStaffAsync(Guid id, StaffListDto dto)
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

        public async Task<bool> DeleteStaffAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.IsActive = false; // Disable
            user.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
