using SportsFacility.DTO;
using SportsFacility.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.Domain.Interface
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetPaymentsAsync();
        Task<(IEnumerable<Payment> Items, int TotalCount)> GetPagedPaymentsAsync(int pageNumber, int pageSize);
        Task<Payment?> CreatePaymentAsync(PaymentListDto dto, Guid loggedInUserId);
        Task<bool> UpdatePaymentAsync(Guid id, PaymentListDto dto);
    }
}
