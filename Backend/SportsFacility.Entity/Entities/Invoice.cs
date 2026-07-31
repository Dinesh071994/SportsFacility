using System;

namespace SportsFacility.Entity.Entities
{
    public class Invoice : BaseEntity
    {
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; } = null!;

        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;

        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
