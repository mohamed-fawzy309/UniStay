using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class University
{
    public int UniversityId { get; set; }

    public string UniversityName { get; set; } = null!;

    public string? City { get; set; }

    public string? Governorate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();

    public virtual ICollection<Dormitory> Dormitories { get; set; } = new List<Dormitory>();
}
