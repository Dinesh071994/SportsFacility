using SportsFacility.API.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SportsFacility.DTO
{

    // DTOs/CreateBookingDto.cs
    public class CreateBookingDto
    {
        [Required(ErrorMessage = "Facility ID is required")]
        public int FacilityId { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        [SportsFacility.DTO.Validations.FutureDate(ErrorMessage = "Start time must be in the future")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        //[GreaterThan(nameof(StartTime), ErrorMessage = "End time must be after start time")]
        public DateTime EndTime { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "Member ID is required")]
        public int MemberId { get; set; }
    }
}
