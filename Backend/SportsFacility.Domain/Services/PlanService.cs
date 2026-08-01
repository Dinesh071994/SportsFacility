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
    public class PlanService : IPlanService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;
        private const string CacheKey = "subscription_plans";

        public PlanService(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<SubscriptionPlan>> GetPlansAsync()
        {
            // Try to get from cache
            var cachedPlans = await _cache.GetRecordAsync<List<SubscriptionPlan>>(CacheKey);
            if (cachedPlans != null)
            {
                return cachedPlans;
            }

            // Fetch from DB
            var plans = await _context.SubscriptionPlans.ToListAsync();

            // Store in cache for 10 minutes
            await _cache.SetRecordAsync(CacheKey, plans, TimeSpan.FromMinutes(10));

            return plans;
        }

        public async Task<SubscriptionPlan> CreatePlanAsync(SubscriptionPlan plan)
        {
            _context.SubscriptionPlans.Add(plan);
            await _context.SaveChangesAsync();

            // Invalidate cache
            await _cache.RemoveAsync(CacheKey);

            return plan;
        }

        public async Task<bool> UpdatePlanAsync(Guid id, SubscriptionPlan plan)
        {
            var existing = await _context.SubscriptionPlans.FindAsync(id);
            if (existing == null) return false;

            existing.Name = plan.Name;
            existing.DurationInMonths = plan.DurationInMonths;
            existing.BillingCycle = plan.BillingCycle;
            existing.MembershipType = plan.MembershipType;
            existing.MaxMembers = plan.MaxMembers;
            existing.Price = plan.Price;
            existing.IsActive = plan.IsActive;

            await _context.SaveChangesAsync();

            // Invalidate cache
            await _cache.RemoveAsync(CacheKey);

            return true;
        }

        public async Task<bool> DeletePlanAsync(Guid id)
        {
            var plan = await _context.SubscriptionPlans.FindAsync(id);
            if (plan == null) return false;

            plan.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();

            // Invalidate cache
            await _cache.RemoveAsync(CacheKey);

            return true;
        }
    }
}
