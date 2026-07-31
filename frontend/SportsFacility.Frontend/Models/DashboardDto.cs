using System.Collections.Generic;

namespace SportsFacility.Frontend.Models
{
    public class DashboardDto
    {
        public decimal MembershipRevenueThisMonth { get; set; }
        public int ActiveMemberships { get; set; }
        public int ExpiringMembershipsSoon { get; set; }
        public int TodaysBookings { get; set; }
        public List<double> WalkinRevenueData { get; set; } = new();
        public List<string> RevenueLabels { get; set; } = new();
        public List<BookingDto> UpcomingBookings { get; set; } = new();
    }

    public class BookingDto 
    {
        public string Title { get; set; }
        public string Status { get; set; } 
        public string Time { get; set; } 
        public string Type { get; set; } 
    }
}
