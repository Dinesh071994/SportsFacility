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
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsAsync()
        {
            return await _context.Payments
                .OrderByDescending(p => p.ModifiedOn ?? p.CreatedOn)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Payment> Items, int TotalCount)> GetPagedPaymentsAsync(int pageNumber, int pageSize)
        {
            var query = _context.Payments
                .OrderByDescending(p => p.ModifiedOn ?? p.CreatedOn);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Payment?> CreatePaymentAsync(PaymentListDto dto, Guid loggedInUserId)
        {
            var userId = loggedInUserId;
            if (userId == Guid.Empty)
            {
                var admin = await _context.Users.FirstOrDefaultAsync(u => u.Role == "Admin");
                userId = admin?.Id ?? Guid.Empty;
            }

            if (userId == Guid.Empty) return null;

            var payment = new Payment
            {
                UserId = userId,
                Amount = dto.Amount,
                Mode = dto.Mode,
                TransactionId = dto.UTR,
                Purpose = dto.Purpose,
                PaymentDate = dto.Date
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> UpdatePaymentAsync(Guid id, PaymentListDto dto)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return false;

            payment.Amount = dto.Amount;
            payment.Mode = dto.Mode;
            payment.TransactionId = dto.UTR;
            payment.Purpose = dto.Purpose;
            payment.PaymentDate = dto.Date;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
