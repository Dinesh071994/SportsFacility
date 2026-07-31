using System;

namespace SportsFacility.Entity.Entities
{
    public class Payment : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public decimal Amount { get; set; }
        public string Mode { get; set; } = "Cash"; // Cash, PhonePe, Card
        public string? TransactionId { get; set; } // UTR for PhonePe
        
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public string Purpose { get; set; } = string.Empty; // Membership, Booking
        public Guid? ReferenceId { get; set; } // UserMembershipId or BookingId

        // Navigation Property
        public Invoice? Invoice { get; set; }
    }
}
