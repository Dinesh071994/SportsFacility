using SportsFacility.DTO;
using SportsFacility.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Interface
{
    public interface IMembershipService
    {
        Task<IEnumerable<UserMembership>> GetMembershipsAsync();
        Task<(IEnumerable<UserMembership> Items, int TotalCount)> GetPagedMembershipsAsync(int pageNumber, int pageSize);
        Task<UserMembership?> CreateMembershipAsync(MemberCreateDto dto);
        Task<bool> UpdateMembershipAsync(Guid id, MemberCreateDto dto);
        Task<bool> DeleteMembershipAsync(Guid id);
    }
}
