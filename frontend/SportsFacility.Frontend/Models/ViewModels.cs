using System.Collections.Generic;

namespace SportsFacility.Frontend.Models
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
    public class StaffModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string Role { get; set; } = "";
        public string Phone { get; set; } = "";
        public bool IsActive { get; set; } = true;
    }

    public class PaymentModel
    {
        public string TransactionId { get; set; } = "";
        public DateTime Date { get; set; }
        
        public DateTime? DateNullable
        {
            get => Date;
            set { if (value.HasValue) Date = value.Value; }
        }
        
        public decimal Amount { get; set; }
        public string Mode { get; set; } = "";
        public string UTR { get; set; } = "";
        public string Purpose { get; set; } = "";
        public DateTime? ModifiedOn { get; set; }
    }

    public class MemberModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Plan { get; set; } = "";
        public string MembershipType { get; set; } = "Individual"; // Individual, Couple, Family, Corporate
        public string QREntryCode { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        public string Status { get; set; } = "Active"; // Active, Expired, Pending
        public DateTime ExpiryDate { get; set; }
        public DateTime? ModifiedOn { get; set; }
        
        public DateTime? ExpiryDateNullable
        {
            get => ExpiryDate;
            set { if (value.HasValue) ExpiryDate = value.Value; }
        }
    }

    public class BookingModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CustomerName { get; set; } = "";
        public string FacilityName { get; set; } = "";
        public string ActivityId { get; set; } = "";
        public DateTime? Date { get; set; } = DateTime.Today;
        public TimeSpan? Time { get; set; } = new TimeSpan(12, 0, 0);
        public string TimeSlot => Time?.ToString(@"hh\:mm") ?? "";
        public bool IsPaid => PaymentMode == "Cash" || PaymentMode == "Online" || PaymentMode == "Paid";
        public string PaymentMode { get; set; } = "Pending";
        public string UTRNumber { get; set; } = "";
        public DateTime? ModifiedOn { get; set; }
    }

    public class ActivityModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public int Capacity { get; set; } = 50;
        public TimeSpan? OpenTime { get; set; } = new TimeSpan(6, 0, 0);
        public TimeSpan? CloseTime { get; set; } = new TimeSpan(22, 0, 0);
        public int NumberOfCourts { get; set; } = 1;
        public int MaxMembersPerCourt { get; set; } = 4;
        public bool IsActive { get; set; } = true;
    }

    public class MembershipModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ActivityId { get; set; } = "";
        public string Name { get; set; } = "";
        public string Type { get; set; } = "Individual"; // Individual, Couple, Family, Corporate
        public string BillingCycle { get; set; } = "Monthly"; // Monthly, Quarterly, Half Yearly, Yearly
        public decimal Price { get; set; }
        public bool IsCouple { get; set; }
        public bool IsActive { get; set; }
    }

    public class ClassModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "Morning Yoga";
        public string TrainerName { get; set; } = "";
        public int Capacity { get; set; } = 20;
        public int Attendance { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.Today;
        
        public DateTime? DateNullable
        {
            get => Date;
            set { if (value.HasValue) Date = value.Value; }
        }
        
        public TimeSpan? StartTime { get; set; } = new TimeSpan(7, 0, 0);
        public TimeSpan? EndTime { get; set; } = new TimeSpan(8, 0, 0);
    }

    public class UserModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "Staff";
        public bool IsActive { get; set; } = true;
        public string? ProfilePicture { get; set; }
    }

    public class MemberCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MembershipType { get; set; } = string.Empty;
        public string PlanId { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
    }
}
