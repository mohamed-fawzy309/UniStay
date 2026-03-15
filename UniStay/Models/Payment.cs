using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? StudentId { get; set; }

    public int? AllocationId { get; set; }

    public string? AcademicYear { get; set; }

    public string? Semester { get; set; }

    public string PaymentCategory { get; set; } = null!;

    public decimal? Amount { get; set; }

    public string? PaymentType { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? ReceiptNumber { get; set; }

    public int? ReceivedBy { get; set; }

    public bool? IsOverdue { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? FirstWarningSentAt { get; set; }

    public DateTime? EvictionNoticeIssuedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Allocation? Allocation { get; set; }

    public virtual Admin? ReceivedByNavigation { get; set; }

    public virtual Student? Student { get; set; }
}
