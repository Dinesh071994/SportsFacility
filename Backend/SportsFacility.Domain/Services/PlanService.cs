using Microsoft.EntityFrameworkCore;
using SportsFacility.Domain.Interface;
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

        public PlanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubscriptionPlan>> GetPlansAsync()
        {
            return await _context.SubscriptionPlans.ToListAsync();
        }

        public async Task<SubscriptionPlan> CreatePlanAsync(SubscriptionPlan plan)
        {
            _context.SubscriptionPlans.Add(plan);
            await _context.SaveChangesAsync();
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
            return true;
        }

        public async Task<bool> DeletePlanAsync(Guid id)
        {
            var plan = await _context.SubscriptionPlans.FindAsync(id);
            if (plan == null) return false;

            plan.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
