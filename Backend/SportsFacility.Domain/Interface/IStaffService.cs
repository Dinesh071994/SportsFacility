using SportsFacility.DTO;
using SportsFacility.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Interface
{
    public interface IStaffService
    {
        Task<IEnumerable<User>> GetStaffAsync();
        Task<User?> CreateStaffAsync(StaffListDto dto);
        Task<bool> UpdateStaffAsync(Guid id, StaffListDto dto);
        Task<bool> DeleteStaffAsync(Guid id);
    }
}
