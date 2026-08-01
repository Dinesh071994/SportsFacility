using Microsoft.EntityFrameworkCore;
using SportsFacility.Domain.Interface;
using SportsFacility.DTO;
using SportsFacility.Entity.Entities;
using SportsFacility.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly ApplicationDbContext _context;

        public MembershipService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserMembership>> GetMembershipsAsync()
        {
            return await _context.UserMemberships
                .AsNoTracking()
                .Include(um => um.User)
                .Include(um => um.SubscriptionPlan)
                .OrderByDescending(um => um.ModifiedOn ?? um.CreatedOn)
                .ToListAsync();
        }

        public async Task<(IEnumerable<UserMembership> Items, int TotalCount)> GetPagedMembershipsAsync(int pageNumber, int pageSize)
        {
            var query = _context.UserMemberships
                .AsNoTracking()
                .Include(um => um.User)
                .Include(um => um.SubscriptionPlan)
                .OrderByDescending(um => um.ModifiedOn ?? um.CreatedOn);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<UserMembership?> CreateMembershipAsync(MemberCreateDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.MobileNumber == dto.Phone);
            if (user == null)
            {
                user = new User
                {
                    FullName = dto.Name,
                    MobileNumber = dto.Phone,
                    Email = dto.Email,
                    Role = "Member",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Welcome@123")
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            var plan = await _context.SubscriptionPlans.FindAsync(Guid.Parse(dto.PlanId));
            if (plan == null) return null;

            var membership = new UserMembership
            {
                UserId = user.Id,
                SubscriptionPlanId = plan.Id,
                StartDate = DateTime.UtcNow,
                ExpiryDate = dto.ExpiryDate,
                Status = "Active",
                QREntryCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()
            };

            _context.UserMemberships.Add(membership);
            await _context.SaveChangesAsync();

            return membership;
        }

        public async Task<bool> UpdateMembershipAsync(Guid id, MemberCreateDto dto)
        {
            var membership = await _context.UserMemberships.Include(m => m.User).FirstOrDefaultAsync(m => m.Id == id);
            if (membership == null) return false;

            var oldPlanId = membership.SubscriptionPlanId;
            var newPlanId = Guid.Parse(dto.PlanId);
            var plan = await _context.SubscriptionPlans.FindAsync(newPlanId);
            if (plan == null) return false;

            membership.User.FullName = dto.Name;
            membership.User.MobileNumber = dto.Phone;
            membership.User.Email = dto.Email;
            membership.SubscriptionPlanId = newPlanId;
            membership.ExpiryDate = dto.ExpiryDate;

            //var isRenewal = dto.ExpiryDate > membership.ExpiryDate;
            //if (isRenewal)
            //{
            //    var payment = new Payment
            //    {
            //        UserId = membership.UserId,
            //        Amount = plan.Price,
            //        Mode = "Cash",
            //        TransactionId = "MEM-UPD-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
            //        Purpose = isRenewal ? "Membership Renewal: " + plan.Name : "Membership Upgrade: " + plan.Name,
            //        PaymentDate = DateTime.UtcNow,
            //        ReferenceId = membership.Id
            //    };
            //    _context.Payments.Add(payment);
            //}

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMembershipAsync(Guid id)
        {
            var membership = await _context.UserMemberships.FindAsync(id);
            if (membership == null) return false;

            membership.Status = "Expired";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
