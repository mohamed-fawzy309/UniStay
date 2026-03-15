using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string? Name { get; set; }

    public string? Role { get; set; }

    public string? Username { get; set; }

    public byte[]? PasswordHash { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int? DormitoryId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();

    public virtual ICollection<Allocation> Allocations { get; set; } = new List<Allocation>();

    public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual Dormitory? Dormitory { get; set; }

    public virtual ICollection<EvictionNotice> EvictionNotices { get; set; } = new List<EvictionNotice>();

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
