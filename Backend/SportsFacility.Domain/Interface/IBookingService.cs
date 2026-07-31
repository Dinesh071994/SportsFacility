using SportsFacility.DTO;
using SportsFacility.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Interface
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetBookingsAsync();
        Task<(IEnumerable<Booking> Items, int TotalCount)> GetPagedBookingsAsync(int pageNumber, int pageSize);
        Task<Booking?> CreateBookingAsync(BookingListDto dto);
        Task<bool> UpdateBookingAsync(Guid id, BookingListDto dto);
        Task<bool> DeleteBookingAsync(Guid id);
    }
}
