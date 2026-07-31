using System;
using System.Collections.Generic;

namespace SportsFacility.Entity.Entities
{
    public class SubscriptionPlan : BaseEntity
    {
        public Guid FacilityId { get; set; }
        public Facility Facility { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        
        // e.g., Monthly (1), Quarterly (3), Half-Yearly (6), Yearly (12)
        public int DurationInMonths { get; set; }
        public string BillingCycle { get; set; } = "Monthly"; 
        
        public string MembershipType { get; set; } = "Individual"; // Individual, Couple, Family, Corporate
        public int MaxMembers { get; set; } = 1; // 1 for Individual, 2 for Couple, N for Family
        
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<UserMembership> UserMemberships { get; set; } = new List<UserMembership>();
    }
}
