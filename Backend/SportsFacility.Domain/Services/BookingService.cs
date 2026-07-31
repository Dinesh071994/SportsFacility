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
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Court)
                    .ThenInclude(c => c.Facility)
                .OrderByDescending(b => b.ModifiedOn ?? b.CreatedOn)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Booking> Items, int TotalCount)> GetPagedBookingsAsync(int pageNumber, int pageSize)
        {
            var query = _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Court)
                    .ThenInclude(c => c.Facility)
                .OrderByDescending(b => b.ModifiedOn ?? b.CreatedOn);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Booking?> CreateBookingAsync(BookingListDto dto)
        {
            // 1. Resolve User (find or create)
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FullName == dto.CustomerName);
            if (user == null)
            {
                user = new User
                {
                    FullName = dto.CustomerName,
                    MobileNumber = "BK-" + Guid.NewGuid().ToString("N").Substring(0, 8),
                    Email = dto.CustomerName.Replace(" ", "").ToLower() + "@example.com",
                    Role = "Customer",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Welcome@123")
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // 2. Resolve Court (find or create)
            var court = await _context.Courts.Include(c => c.Facility).FirstOrDefaultAsync(c => c.Name == dto.FacilityName);
            if (court == null)
            {
                // Create a default facility for the sport if not exists
                var category = dto.FacilityName.Contains("Turf") ? "Football" : "Badminton";
                var facilityName = dto.FacilityName.Contains("Turf") ? "Soccer Turf Arena" : "Badminton Arena";
                
                var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Name == facilityName);
                if (facility == null)
                {
                    facility = new Facility
                    {
                        Name = facilityName,
                        Category = category,
                        Capacity = 20,
                        OpenTime = TimeSpan.FromHours(6),
                        CloseTime = TimeSpan.FromHours(22),
                        IsActive = true,
                        CreatedBy = "System"
                    };
                    _context.Facilities.Add(facility);
                    await _context.SaveChangesAsync();
                }

                court = new Court
                {
                    FacilityId = facility.Id,
                    Name = dto.FacilityName,
                    IsActive = true,
                    CreatedBy = "System"
                };
                _context.Courts.Add(court);
                await _context.SaveChangesAsync();
            }

            var startTime = dto.Time ?? TimeSpan.FromHours(12);
            var booking = new Booking
            {
                UserId = user.Id,
                CourtId = court.Id,
                Date = dto.Date ?? DateTime.UtcNow.Date,
                StartTime = startTime,
                EndTime = startTime.Add(TimeSpan.FromHours(1)),
                Status = "Confirmed",
                PaymentStatus = dto.PaymentMode == "Pending" ? "Pending" : "Paid"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // 3. Create Payment if paid
            if (dto.PaymentMode == "Cash" || dto.PaymentMode == "Online")
            {
                var payment = new Payment
                {
                    UserId = user.Id,
                    Amount = 250.0m, // Standard booking fee
                    Mode = dto.PaymentMode,
                    TransactionId = dto.UTRNumber,
                    Purpose = "Booking",
                    PaymentDate = DateTime.UtcNow,
                    ReferenceId = booking.Id
                };
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
            }

            // Reload relationships for return mapping
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Court)
                    .ThenInclude(c => c.Facility)
                .FirstOrDefaultAsync(b => b.Id == booking.Id);
        }

        public async Task<bool> UpdateBookingAsync(Guid id, BookingListDto dto)
        {
            var booking = await _context.Bookings.Include(b => b.User).Include(b => b.Court).FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null) return false;

            booking.User.FullName = dto.CustomerName;
            booking.Date = dto.Date ?? booking.Date;
            booking.StartTime = dto.Time ?? booking.StartTime;
            booking.EndTime = booking.StartTime.Add(TimeSpan.FromHours(1));
            booking.PaymentStatus = dto.PaymentMode == "Pending" ? "Pending" : "Paid";
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBookingAsync(Guid id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return false;

            booking.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
