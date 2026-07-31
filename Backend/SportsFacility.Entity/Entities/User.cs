using System;
using System.Collections.Generic;

namespace SportsFacility.Entity.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty; // Unique
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? EmergencyContact { get; set; }
        
        public string Role { get; set; } = "Customer"; // SuperAdmin, BranchAdmin, Receptionist, Trainer, Customer
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public ICollection<UserMembership> PrimaryMemberships { get; set; } = new List<UserMembership>();
        public ICollection<UserMembershipDependent> DependentMemberships { get; set; } = new List<UserMembershipDependent>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<ClassAttendance> ClassAttendances { get; set; } = new List<ClassAttendance>();
    }
}
