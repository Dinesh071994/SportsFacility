using SportsFacility.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Interface
{
    public interface IPlanService
    {
        Task<IEnumerable<SubscriptionPlan>> GetPlansAsync();
        Task<SubscriptionPlan> CreatePlanAsync(SubscriptionPlan plan);
        Task<bool> UpdatePlanAsync(Guid id, SubscriptionPlan plan);
        Task<bool> DeletePlanAsync(Guid id);
    }
}
