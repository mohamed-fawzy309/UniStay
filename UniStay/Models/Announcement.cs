using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class Announcement
{
    public int AnnouncementId { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public string? AcademicYear { get; set; }

    public int? UniversityId { get; set; }

    public int? DormitoryId { get; set; }

    public bool? IsPublic { get; set; }

    public DateTime? PublishedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<AnnouncementAttachment> AnnouncementAttachments { get; set; } = new List<AnnouncementAttachment>();

    public virtual Admin? CreatedByNavigation { get; set; }

    public virtual Dormitory? Dormitory { get; set; }

    public virtual University? University { get; set; }
}
