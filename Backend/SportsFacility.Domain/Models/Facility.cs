using System;
using System.Collections.Generic;
using System.Text;

namespace SportsFacility.Domain.Models
{
    // Models/Facility.cs
    public class Facility
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // tennis, basketball, football, swimming
        public string Description { get; set; } = string.Empty;
        public decimal HourlyRate { get; set; }
        public int Capacity { get; set; }
        public bool HasLighting { get; set; }
        public bool IsIndoor { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public TimeSpan OperatingHours { get; set; }
        public List<Booking> Bookings { get; set; } = new();
        public List<FacilityEquipment> Equipment { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
