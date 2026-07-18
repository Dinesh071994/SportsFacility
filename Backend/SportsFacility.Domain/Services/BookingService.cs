using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportsFacility.Domain.Infra;
using SportsFacility.Domain.Interface;
using SportsFacility.Domain.Models;
using SportsFacility.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Services
{
    public class BookingService : IBookingService
    {
        private readonly SportsFacilityDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingService> _logger;

        public BookingService(SportsFacilityDbContext context, IMapper mapper, ILogger<BookingService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Member)
                .Include(b => b.Facility)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.Member)
                .Include(b => b.Facility)
                .ToListAsync();
        }

        public async Task<Booking> CreateAsync(Booking booking)
        {
            booking.CreatedAt = DateTime.UtcNow;
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking> UpdateAsync(int id, Booking booking)
        {
            var existing = await _context.Bookings.FindAsync(id);
            if (existing == null) return null;

            // update allowed fields
            existing.StartTime = booking.StartTime;
            existing.EndTime = booking.EndTime;
            existing.Notes = booking.Notes;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Bookings.FindAsync(id);
            if (existing == null) return false;
            _context.Bookings.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Booking> CreateBookingAsync(CreateBookingDto bookingDto)
        {
            // Validate facility availability
            var facility = await _context.Facilities.FindAsync(bookingDto.FacilityId);
            if (facility == null)
                throw new Exception("Facility not found");

            // Check for conflicting bookings
            var conflicting = await _context.Bookings
                .Where(b => b.FacilityId == bookingDto.FacilityId &&
                           b.Status != "Cancelled" &&
                           (b.StartTime < bookingDto.EndTime && b.EndTime > bookingDto.StartTime))
                .ToListAsync();

            if (conflicting.Any())
                throw new Exception("Facility is not available during the requested time");

            var booking = _mapper.Map<Booking>(bookingDto);
            booking.CreatedAt = DateTime.UtcNow;
            booking.Status = "Pending";
            booking.PaymentStatus = "Pending";

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            booking.TransactionId = GenerateTransactionId();

            _logger.LogInformation($"Booking created for facility {facility.Name} by member {booking.MemberId}");

            return booking;
        }

        public async Task<IEnumerable<Booking>> GetMyBookingsAsync(int userId)
        {
            return await _context.Bookings
                .Where(b => b.MemberId == userId)
                .Include(b => b.Facility)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByFacilityAsync(int facilityId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Bookings.Where(b => b.FacilityId == facilityId).AsQueryable();
            if (startDate.HasValue) query = query.Where(b => b.StartTime >= startDate.Value);
            if (endDate.HasValue) query = query.Where(b => b.EndTime <= endDate.Value);
            return await query.Include(b => b.Member).ToListAsync();
        }

        public async Task<bool> ConfirmBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return false;
            booking.Status = "Confirmed";
            booking.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TimeSlot>> GetAvailableSlotsAsync(int facilityId, DateTime date)
        {
            var startTime = date.Date.AddHours(7); // 7 AM
            var endTime = date.Date.AddHours(22); // 10 PM
            var slots = new List<TimeSlot>();
            var slotDuration = TimeSpan.FromHours(1);

            for (var currentTime = startTime; currentTime < endTime; currentTime += slotDuration)
            {
                var slotEndTime = currentTime + slotDuration;

                var isAvailable = !_context.Bookings
                    .Any(b => b.FacilityId == facilityId &&
                              b.Status != "Cancelled" &&
                              b.StartTime < slotEndTime &&
                              b.EndTime > currentTime);

                slots.Add(new TimeSlot
                {
                    StartTime = currentTime,
                    EndTime = slotEndTime,
                    IsAvailable = isAvailable
                });
            }

            return slots;
        }

        private string GenerateTransactionId()
        {
            return $"TXN{DateTime.UtcNow:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";
        }
    }

    public class TimeSlot
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
