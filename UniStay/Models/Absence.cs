using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class Absence
{
    public int AbsenceId { get; set; }

    public int StudentId { get; set; }

    public int? AllocationId { get; set; }

    public string? AbsenceType { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public int? DaysCount { get; set; }

    public bool? IsWeekendIncluded { get; set; }

    public string? Reason { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Allocation? Allocation { get; set; }

    public virtual Admin? ApprovedByNavigation { get; set; }

    public virtual Student Student { get; set; } = null!;
}
