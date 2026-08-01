using System.Collections.Generic;

namespace SportsFacility.DTO
{
    public class PaginatedResultDto<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
    }

    public class ActivityDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public int NumberOfCourts { get; set; }
        public int MaxMembersPerCourt { get; set; }
        public bool IsActive { get; set; }
    }

    public class MembershipPlanDto
    {
        public string Id { get; set; } = string.Empty;
        public string ActivityId { get; set; } = string.Empty; // Maps to FacilityId
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "Individual"; 
        public string BillingCycle { get; set; } = "Monthly"; 
        public decimal Price { get; set; }
        public bool IsCouple { get; set; } // Maps to MaxMembers == 2
        public bool IsActive { get; set; }
    }

    public class MemberListDto
    {
        public string Id { get; set; } = string.Empty; // Maps to UserMembershipId
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Plan { get; set; } = string.Empty; // Plan name
        public string MembershipType { get; set; } = string.Empty;
        public string QREntryCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class MemberCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MembershipType { get; set; } = string.Empty;
        public string PlanId { get; set; } = string.Empty; // SubscriptionPlanId
        public DateTime ExpiryDate { get; set; }
    }

    public class ClassScheduleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string TrainerName { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int Attendance { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }

    public class BookingListDto
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string FacilityName { get; set; } = string.Empty;
        public string ActivityId { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public string UTRNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? ModifiedOn { get; set; }
    }

    public class PaymentListDto
    {
        public string TransactionId { get; set; } = string.Empty; // Maps to Payment.Id
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Mode { get; set; } = string.Empty;
        public string UTR { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public DateTime? ModifiedOn { get; set; }
    }

    public class StaffListDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
