using System;
using System.Collections.Generic;
using System.Text;

namespace SportsFacility.Domain.Models
{
    public class FacilityEquipment
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public Facility Facility { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // racket, ball, net, scoreboard
        public int Quantity { get; set; }
        public decimal RentalRate { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string? MaintenanceNotes { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public List<Booking> Bookings { get; set; } = new();
    }
}
