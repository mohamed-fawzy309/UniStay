using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class Violation
{
    public int ViolationId { get; set; }

    public int? StudentId { get; set; }

    public string? ViolationType { get; set; }

    public string? Description { get; set; }

    public DateOnly? ViolationDate { get; set; }

    public string? Penalty { get; set; }

    public decimal? PenaltyAmount { get; set; }

    public bool? IsPaid { get; set; }

    public int? RecordedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Admin? RecordedByNavigation { get; set; }

    public virtual Student? Student { get; set; }
}
