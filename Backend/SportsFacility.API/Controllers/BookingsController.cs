using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsFacility.Domain.Interface;
using SportsFacility.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.API.Controllers
{
    public class BookingsController : BaseApiController
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingsController(IBookingService bookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetBookings([FromQuery] string? type = null, [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            if(type != null)
            {
                var bookingsByType = await _bookingService.GetBookingsAsync(type);
                return Ok(_mapper.Map<IEnumerable<BookingListDto>>(bookingsByType));
            }

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                var (items, totalCount) = await _bookingService.GetPagedBookingsAsync(pageNumber.Value, pageSize.Value);
                var mappedItems = _mapper.Map<IEnumerable<BookingListDto>>(items);
                return Ok(new PaginatedResultDto<BookingListDto>
                {
                    Items = mappedItems,
                    TotalCount = totalCount
                });
            }

            var bookings = await _bookingService.GetBookingsAsync();
            return Ok(_mapper.Map<IEnumerable<BookingListDto>>(bookings));
        }

        [HttpPost]
        public async Task<ActionResult<BookingListDto>> CreateBooking(BookingListDto dto)
        {
            var booking = await _bookingService.CreateBookingAsync(dto);
            if (booking == null) return BadRequest("Could not create booking");

            return CreatedAtAction(nameof(GetBookings), new { id = booking.Id }, _mapper.Map<BookingListDto>(booking));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(Guid id, BookingListDto dto)
        {
            var result = await _bookingService.UpdateBookingAsync(id, dto);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var result = await _bookingService.DeleteBookingAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
