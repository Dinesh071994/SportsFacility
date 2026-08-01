using Microsoft.EntityFrameworkCore;
using SportsFacility.Domain.Interface;
using SportsFacility.Entity.Entities;
using SportsFacility.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Services
{
    public class FacilityService : IFacilityService
    {
        private readonly ApplicationDbContext _context;

        public FacilityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Facility>> GetFacilitiesAsync()
        {
            return await _context.Facilities
                .ToListAsync();
        }

        public async Task<Facility> CreateFacilityAsync(Facility facility, int numberOfCourts)
        {
            _context.Facilities.Add(facility);
            await _context.SaveChangesAsync();
            return facility;
        }

        public async Task<bool> UpdateFacilityAsync(Guid id, Facility facility)
        {
            var existing = await _context.Facilities.FindAsync(id);
            if (existing == null) return false;

            existing.Name = facility.Name;
            existing.Category = facility.Category;
            existing.Capacity = facility.Capacity;
            existing.OpenTime = facility.OpenTime;
            existing.CloseTime = facility.CloseTime;
            existing.IsActive = facility.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFacilityAsync(Guid id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return false;

            facility.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
