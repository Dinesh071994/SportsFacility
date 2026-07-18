using System;
using System.Collections.Generic;
using System.Text;

namespace SportsFacility.DTO
{
    // DTOs/MemberDto.cs
    public class MemberDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? AadhaarNumber { get; set; }
        public string MembershipType { get; set; } = "None";
        public DateTime? MembershipStartDate { get; set; }
        public DateTime? MembershipEndDate { get; set; }
        public decimal MembershipFee { get; set; }
        public bool IsActive { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
