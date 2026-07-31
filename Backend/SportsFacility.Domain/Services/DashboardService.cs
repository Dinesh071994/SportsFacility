using Microsoft.EntityFrameworkCore;
using SportsFacility.Domain.Interface;
using SportsFacility.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetDashboardDataAsync()
        {
            var today = DateTime.UtcNow.Date;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var inSevenDays = today.AddDays(7);

            // 1. Membership Revenue This Month
            var membershipRevenue = await _context.Payments
                .Where(p => p.Purpose == "Membership" && p.PaymentDate >= startOfMonth)
                .SumAsync(p => p.Amount);

            // 2. Active Memberships
            var activeMemberships = await _context.UserMemberships
                .CountAsync(um => um.Status == "Active" && um.ExpiryDate > DateTime.UtcNow);

            // 3. Expiring Memberships Soon (Next 7 Days)
            var expiringMemberships = await _context.UserMemberships
                .CountAsync(um => um.Status == "Active" && um.ExpiryDate > DateTime.UtcNow && um.ExpiryDate <= inSevenDays);

            // 4. Today's Bookings
            var todaysBookings = await _context.Bookings
                .CountAsync(b => b.Date.Date == today);

            // 5. Walk-in Revenue Analytics (Last 7 Days)
            var walkinRevenueData = new List<double>();
            var revenueLabels = new List<string>();

            for (int i = 6; i >= 0; i--)
            {
                var targetDate = today.AddDays(-i);
                var revenueForDay = await _context.Payments
                    .Where(p => p.Purpose == "Booking" && p.PaymentDate.Date == targetDate)
                    .SumAsync(p => p.Amount);

                walkinRevenueData.Add((double)revenueForDay);
                revenueLabels.Add(targetDate.ToString("ddd"));
            }

            // 6. Upcoming Bookings
            var upcomingBookings = await _context.Bookings
                .Include(b => b.Court)
                .Where(b => b.Date >= today)
                .OrderBy(b => b.Date).ThenBy(b => b.StartTime)
                .Take(5)
                .Select(b => new
                {
                    Title = $"{b.Court.Name} - Booking",
                    Status = b.PaymentStatus == "Paid" ? "Confirmed" : "Pending Pay",
                    Time = $"{b.StartTime:hh\\:mm} - {b.EndTime:hh\\:mm}",
                    Type = "Walk-in"
                })
                .ToListAsync();

            return new
            {
                MembershipRevenueThisMonth = membershipRevenue,
                ActiveMemberships = activeMemberships,
                ExpiringMembershipsSoon = expiringMemberships,
                TodaysBookings = todaysBookings,
                WalkinRevenueData = walkinRevenueData,
                RevenueLabels = revenueLabels,
                UpcomingBookings = upcomingBookings
            };
        }
    }
}
