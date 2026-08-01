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
            var sevenDaysAgo = today.AddDays(-6);
            var tomorrow = today.AddDays(1);

            // 1. Membership Revenue This Month
            var membershipRevenue = await _context.Payments
                .AsNoTracking()
                .Where(p => p.Purpose == "Membership" && p.PaymentDate >= startOfMonth)
                .SumAsync(p => p.Amount);

            // 2. Active Memberships
            var activeMemberships = await _context.UserMemberships
                .AsNoTracking()
                .CountAsync(um => um.Status == "Active" && um.ExpiryDate > DateTime.UtcNow);

            // 3. Expiring Memberships Soon (Next 7 Days)
            var expiringMemberships = await _context.UserMemberships
                .AsNoTracking()
                .CountAsync(um => um.Status == "Active" && um.ExpiryDate > DateTime.UtcNow && um.ExpiryDate <= inSevenDays);

            // 4. Today's Bookings (using range query to utilize indexes)
            var todaysBookings = await _context.Bookings
                .AsNoTracking()
                .CountAsync(b => b.Date >= today && b.Date < tomorrow);

            // 5. Batch Walk-in Revenue Analytics (Last 7 Days) in a single DB query
            var walkinRevenueData = new List<double>();
            var revenueLabels = new List<string>();

            var lastSevenDaysPayments = await _context.Payments
                .AsNoTracking()
                .Where(p => p.Purpose == "Booking" && p.PaymentDate >= sevenDaysAgo && p.PaymentDate < tomorrow)
                .Select(p => new { p.PaymentDate, p.Amount })
                .ToListAsync();

            var revenueByDate = lastSevenDaysPayments
                .GroupBy(p => p.PaymentDate.Date)
                .ToDictionary(g => g.Key, g => g.Sum(p => (double)p.Amount));

            for (int i = 6; i >= 0; i--)
            {
                var targetDate = today.AddDays(-i);
                revenueByDate.TryGetValue(targetDate, out double revenueForDay);
                walkinRevenueData.Add(revenueForDay);
                revenueLabels.Add(targetDate.ToString("ddd"));
            }

            // 6. Upcoming Bookings
            var upcomingBookings = await _context.Bookings
                .AsNoTracking()
                .Include(b => b.Facility)
                .Where(b => b.Date >= today)
                .OrderBy(b => b.Date).ThenBy(b => b.StartTime)
                .Take(5)
                .Select(b => new
                {
                    Title = $"{b.Facility.Name} - Booking",
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
