using System;
using System.Collections.Generic;

namespace SportsFacility.Entity.Entities
{
    public class ClassSchedule : BaseEntity
    {
        public Guid FacilityId { get; set; }
        public Facility Facility { get; set; } = null!;

        public Guid? TrainerId { get; set; }
        public User? Trainer { get; set; }

        public string Name { get; set; } = string.Empty;
        
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        
        public int Capacity { get; set; }

        public ICollection<ClassAttendance> Attendances { get; set; } = new List<ClassAttendance>();
    }
}
