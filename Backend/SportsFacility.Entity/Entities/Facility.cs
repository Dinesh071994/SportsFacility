using System;
using System.Collections.Generic;

namespace SportsFacility.Entity.Entities
{
    public class Facility : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Gym, Badminton, Zumba, Yoga
        public int Capacity { get; set; }
        
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public ICollection<SubscriptionPlan> SubscriptionPlans { get; set; } = new List<SubscriptionPlan>();
        public ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>();
    }
}
