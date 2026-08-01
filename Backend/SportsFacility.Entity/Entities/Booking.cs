using System;

namespace SportsFacility.Entity.Entities
{
    public class Booking : BaseEntity
    {
        public Guid FacilityId { get; set; }
        public Facility Facility { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string Status { get; set; } = "Confirmed"; // Confirmed, Cancelled
        public string PaymentStatus { get; set; } = "Pending"; // Pending, Paid
    }
}
