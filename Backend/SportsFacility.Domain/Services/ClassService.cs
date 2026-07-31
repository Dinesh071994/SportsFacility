using Microsoft.EntityFrameworkCore;
using SportsFacility.Domain.Interface;
using SportsFacility.DTO;
using SportsFacility.Entity.Entities;
using SportsFacility.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Services
{
    public class ClassService : IClassService
    {
        private readonly ApplicationDbContext _context;

        public ClassService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClassSchedule>> GetClassesAsync()
        {
            return await _context.ClassSchedules
                .Include(c => c.Trainer)
                .Include(c => c.Facility)
                .Include(c => c.Attendances)
                .ToListAsync();
        }

        public async Task<ClassSchedule?> CreateClassAsync(ClassScheduleDto dto)
        {
            // 1. Resolve Trainer (find or create)
            var trainer = await _context.Users.FirstOrDefaultAsync(u => u.FullName == dto.TrainerName);
            if (trainer == null)
            {
                trainer = new User
                {
                    FullName = dto.TrainerName,
                    MobileNumber = "TR-" + Guid.NewGuid().ToString("N").Substring(0, 8),
                    Email = dto.TrainerName.Replace(" ", "").ToLower() + "@example.com",
                    Role = "Trainer",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Trainer@123")
                };
                _context.Users.Add(trainer);
                await _context.SaveChangesAsync();
            }

            // 2. Resolve Facility (find or create)
            var facilityName = "General Class Area";
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Name == facilityName);
            if (facility == null)
            {
                facility = new Facility
                {
                    Name = facilityName,
                    Category = "General",
                    Capacity = dto.Capacity > 0 ? dto.Capacity : 100,
                    OpenTime = TimeSpan.FromHours(6),
                    CloseTime = TimeSpan.FromHours(22),
                    IsActive = true,
                    CreatedBy = "System"
                };
                _context.Facilities.Add(facility);
                await _context.SaveChangesAsync();
            }

            var classSchedule = new ClassSchedule
            {
                Name = dto.Name,
                TrainerId = trainer.Id,
                FacilityId = facility.Id,
                Date = dto.Date.Date,
                StartTime = dto.StartTime ?? TimeSpan.FromHours(8),
                EndTime = dto.EndTime ?? TimeSpan.FromHours(9),
                Capacity = dto.Capacity
            };

            _context.ClassSchedules.Add(classSchedule);
            await _context.SaveChangesAsync();

            // Load relations for response mapping
            return await _context.ClassSchedules
                .Include(c => c.Trainer)
                .Include(c => c.Facility)
                .Include(c => c.Attendances)
                .FirstOrDefaultAsync(c => c.Id == classSchedule.Id);
        }

        public async Task<bool> UpdateClassAsync(Guid id, ClassScheduleDto dto)
        {
            var classSchedule = await _context.ClassSchedules.Include(c => c.Trainer).FirstOrDefaultAsync(c => c.Id == id);
            if (classSchedule == null) return false;

            classSchedule.Name = dto.Name;
            classSchedule.Capacity = dto.Capacity;
            classSchedule.Date = dto.Date.Date;
            classSchedule.StartTime = dto.StartTime ?? classSchedule.StartTime;
            classSchedule.EndTime = dto.EndTime ?? classSchedule.EndTime;
            classSchedule.Trainer.FullName = dto.TrainerName;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteClassAsync(Guid id)
        {
            var classSchedule = await _context.ClassSchedules.FindAsync(id);
            if (classSchedule == null) return false;

            classSchedule.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAttendanceAsync(Guid id)
        {
            var classSchedule = await _context.ClassSchedules.Include(c => c.Attendances).FirstOrDefaultAsync(c => c.Id == id);
            if (classSchedule == null || classSchedule.Attendances.Count >= classSchedule.Capacity) return false;

            // Mark attendance for a random member / user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Role == "Member");
            if (user == null) return false;

            var attendance = new ClassAttendance
            {
                ClassScheduleId = id,
                UserId = user.Id
            };

            _context.ClassAttendances.Add(attendance);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
