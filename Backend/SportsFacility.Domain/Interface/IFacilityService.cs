using SportsFacility.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Interface
{
    public interface IFacilityService
    {
        Task<IEnumerable<Facility>> GetFacilitiesAsync();
        Task<Facility> CreateFacilityAsync(Facility facility, int numberOfCourts);
        Task<bool> UpdateFacilityAsync(Guid id, Facility facility);
        Task<bool> DeleteFacilityAsync(Guid id);
    }
}
