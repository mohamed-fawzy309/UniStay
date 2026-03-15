using UniStay.Models;

namespace UniStay.ViewModels
{
    public class PaymentsViewModel
    {
        /// <summary>Application that has Status = "PendingPayment" (no Payment record yet).</summary>
        public Application? PendingApplication { get; set; }

        /// <summary>Payment records that are due but not yet paid (PaymentDate == null).</summary>
        public List<Payment> PendingPayments { get; set; } = new();

        /// <summary>Payments where IsOverdue == true or DueDate has passed without a PaymentDate.</summary>
        public List<Payment> OverduePayments { get; set; } = new();

        /// <summary>Full payment history ordered by date desc.</summary>
        public List<Payment> AllPayments { get; set; } = new();

        public decimal TotalPaid { get; set; }
        public decimal TotalOverdue { get; set; }
    }
}