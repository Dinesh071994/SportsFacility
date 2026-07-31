using SportsFacility.DTO;
using SportsFacility.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Interface
{
    public interface IClassService
    {
        Task<IEnumerable<ClassSchedule>> GetClassesAsync();
        Task<ClassSchedule?> CreateClassAsync(ClassScheduleDto dto);
        Task<bool> UpdateClassAsync(Guid id, ClassScheduleDto dto);
        Task<bool> DeleteClassAsync(Guid id);
        Task<bool> MarkAttendanceAsync(Guid id);
    }
}
