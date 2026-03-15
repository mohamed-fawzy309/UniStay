using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class ApplicationSchedule
{
    public int ScheduleId { get; set; }

    public string AcademicYear { get; set; } = null!;

    public string StudentCategory { get; set; } = null!;

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public int? DormitoryId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Dormitory? Dormitory { get; set; }
}
