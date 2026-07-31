using System;

namespace SportsFacility.Entity.Entities
{
    public class ClassAttendance : BaseEntity
    {
        public Guid ClassScheduleId { get; set; }
        public ClassSchedule ClassSchedule { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public string Status { get; set; } = "Booked"; // Booked, Attended, NoShow
    }
}
