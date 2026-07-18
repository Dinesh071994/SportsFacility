using System;
using System.Collections.Generic;
using System.Text;

namespace SportsFacility.Domain.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
        public int FacilityId { get; set; }
        public Facility Facility { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Completed, Cancelled
        public string PaymentStatus { get; set; } = "Pending"; // Pending, Paid, Failed, Refunded
        public string PaymentMethod { get; set; } = string.Empty; // UPI, Card, Cash, NetBanking
        public string TransactionId { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
