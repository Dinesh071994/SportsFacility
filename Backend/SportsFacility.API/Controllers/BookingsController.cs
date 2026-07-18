using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsFacility.Domain.Interface;
using SportsFacility.Domain.Models;
using SportsFacility.DTO;

namespace SportsFacility.API.Controllers
{

    // Controllers/BookingsController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBaseService<Booking> _bookingService;
        private readonly IMapper _mapper;

        public BookingsController(IBaseService<Booking> bookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _mapper = mapper;
        }

        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> GetMyBookings()
        //{
        //    var userId = GetCurrentUserId();
        //    var bookings = await _bookingService.GetMyBookingsAsync(userId);
        //    var bookingDtos = _mapper.Map<List<BookingDto>>(bookings);

        //    return Ok(new { data = bookingDtos, count = bookingDtos.Count });
        //}

        //[HttpGet("facility/{facilityId}")]
        //[Authorize(Roles = "Admin,Manager,Staff")]
        //public async Task<IActionResult> GetBookingsByFacility(int facilityId, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        //{
        //    var bookings = await _bookingService.GetByFacilityAsync(facilityId, startDate, endDate);
        //    var bookingDtos = _mapper.Map<List<BookingDto>>(bookings);

        //    return Ok(new { data = bookingDtos, count = bookingDtos.Count });
        //}

        //[HttpPost]
        //[Authorize(Roles = "Admin,Manager,Staff")]
        //public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto bookingDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    try
        //    {
        //        var result = await _bookingService.CreateBookingAsync(bookingDto);
        //        return Ok(new
        //        {
        //            message = "Booking created successfully",
        //            bookingId = result.Id,
        //            transactionId = result.TransactionId
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { error = ex.Message });
        //    }
        //}

        //[HttpPut("{id}/confirm")]
        //[Authorize(Roles = "Admin,Manager,Staff")]
        //public async Task<IActionResult> ConfirmBooking(int id)
        //{
        //    var result = await _bookingService.ConfirmBookingAsync(id);
        //    if (result == null)
        //        return NotFound(new { error = "Booking not found" });

        //    return Ok(new { message = "Booking confirmed successfully" });
        //}

        //[HttpGet("available/{facilityId}")]
        //[Authorize(Roles = "Admin,Manager,Staff")]
        //public async Task<IActionResult> GetAvailableSlots(int facilityId, DateTime date)
        //{
        //    var availableSlots = await _bookingService.GetAvailableSlotsAsync(facilityId, date);

        //    return Ok(new
        //    {
        //        facilityId = facilityId,
        //        date = date,
        //        availableSlots = availableSlots
        //    });
        //}
    }
}
