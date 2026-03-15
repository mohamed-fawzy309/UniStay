using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class Allocation
{
    public int AllocationId { get; set; }

    public int ApplicationId { get; set; }

    public int StudentId { get; set; }

    public int RoomId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateTime AllocatedAt { get; set; }
    public int BedNumber { get; set; }

    public string? AcademicYear { get; set; }

    public string? Semester { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public bool? IsActive { get; set; }

    public int? AllocatedBy { get; set; }

    public string? MealPlanAllocated { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? EndedAt { get; set; }

    public bool? IsDeleted { get; set; }
    public string? Status { get; set; }  // "Reserved" / "Active"

    public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();

    public virtual Admin? AllocatedByNavigation { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual ICollection<EvictionNotice> EvictionNotices { get; set; } = new List<EvictionNotice>();

    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Room Room { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
