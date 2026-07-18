using SportsFacility.Domain.Models;
using SportsFacility.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsFacility.Domain.Interface
{
    // Services/BookingService.cs
    public interface IBookingService : IBaseService<Booking>
    {
        
        Task<Booking> CreateBookingAsync(DTO.CreateBookingDto bookingDto);
        Task<IEnumerable<Booking>> GetMyBookingsAsync(int userId);
        Task<IEnumerable<Booking>> GetByFacilityAsync(int facilityId, DateTime? startDate = null, DateTime? endDate = null);
        Task<bool> ConfirmBookingAsync(int id);
        Task<List<TimeSlot>> GetAvailableSlotsAsync(int facilityId, DateTime date);
    }

    

}
