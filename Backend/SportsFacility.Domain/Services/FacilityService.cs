using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SportsFacility.Domain.Interface;
using SportsFacility.Domain.Helpers;
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
        private readonly IDistributedCache _cache;
        private const string CacheKey = "facilities";

        public FacilityService(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Facility>> GetFacilitiesAsync()
        {
            // Try to get from cache
            var cachedFacilities = await _cache.GetRecordAsync<List<Facility>>(CacheKey);
            if (cachedFacilities != null)
            {
                return cachedFacilities;
            }

            // Fetch from DB
            var facilities = await _context.Facilities.ToListAsync();

            // Store in cache for 10 minutes
            await _cache.SetRecordAsync(CacheKey, facilities, TimeSpan.FromMinutes(10));

            return facilities;
        }

        public async Task<Facility> CreateFacilityAsync(Facility facility, int numberOfCourts)
        {
            _context.Facilities.Add(facility);
            await _context.SaveChangesAsync();

            // Invalidate cache
            await _cache.RemoveAsync(CacheKey);

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

            // Invalidate cache
            await _cache.RemoveAsync(CacheKey);

            return true;
        }

        public async Task<bool> DeleteFacilityAsync(Guid id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return false;

            facility.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();

            // Invalidate cache
            await _cache.RemoveAsync(CacheKey);

            return true;
        }
    }
}
