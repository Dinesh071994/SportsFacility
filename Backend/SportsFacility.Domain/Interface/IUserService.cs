using SportsFacility.DTO;
using SportsFacility.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> CreateUserAsync(StaffListDto dto);
        Task<bool> UpdateUserAsync(Guid id, StaffListDto dto);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> ResetPasswordAsync(Guid id, string newPassword);
    }
}
