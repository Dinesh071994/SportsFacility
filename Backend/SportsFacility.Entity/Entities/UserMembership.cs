using System;
using System.Collections.Generic;

namespace SportsFacility.Entity.Entities
{
    public class UserMembership : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid SubscriptionPlanId { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        
        public string Status { get; set; } = "Active"; // Active, Expired, Pending
        public string QREntryCode { get; set; } = string.Empty;

        public ICollection<UserMembershipDependent> Dependents { get; set; } = new List<UserMembershipDependent>();
    }
}
