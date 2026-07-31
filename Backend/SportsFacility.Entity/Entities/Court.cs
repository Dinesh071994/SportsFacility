using System;
using System.Collections.Generic;

namespace SportsFacility.Entity.Entities
{
    public class Court : BaseEntity
    {
        public Guid FacilityId { get; set; }
        public Facility Facility { get; set; } = null!;

        public string Name { get; set; } = string.Empty; // e.g. "Court 1"
        public bool IsActive { get; set; } = true;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
