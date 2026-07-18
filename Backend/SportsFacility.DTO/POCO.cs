namespace SportsFacility.DTO
{
    public class EquipmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int FacilityId { get; set; }
    }

    public class RegisterMemberDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? AadhaarNumber { get; set; }
    }

    public class RegisterUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    // DTOs/FacilityDto.cs
    public class FacilityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal HourlyRate { get; set; }
        public int Capacity { get; set; }
        public bool HasLighting { get; set; }
        public bool IsIndoor { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public TimeSpan OperatingHours { get; set; }
        public List<EquipmentDto> Equipment { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // DTOs/BookingDto.cs
    public class BookingDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public int FacilityId { get; set; }
        public string FacilityName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public string PaymentStatus { get; set; } = "Pending";
        public string PaymentMethod { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }
    
}
