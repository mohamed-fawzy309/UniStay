using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class EvictionNotice
{
    public int EvictionId { get; set; }

    public int? StudentId { get; set; }

    public int? AllocationId { get; set; }

    public DateTime? NoticeDate { get; set; }

    public DateTime? DueDate { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Allocation? Allocation { get; set; }

    public virtual Admin? CreatedByNavigation { get; set; }

    public virtual Student? Student { get; set; }
}
