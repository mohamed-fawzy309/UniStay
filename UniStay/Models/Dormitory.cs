using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class Dormitory
{
    public int DormitoryId { get; set; }

    public string DormitoryName { get; set; } = null!;

    public string? Type { get; set; }

    public string? Address { get; set; }

    public int UniversityId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();

    public virtual ICollection<ApplicationSchedule> ApplicationSchedules { get; set; } = new List<ApplicationSchedule>();

    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();

    public virtual University University { get; set; } = null!;
}
