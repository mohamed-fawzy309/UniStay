using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class MaintenanceRequest
{
    public int RequestId { get; set; }

    public int? RoomId { get; set; }

    public int? StudentId { get; set; }

    public string? IssueType { get; set; }

    public string? Description { get; set; }

    public string? Priority { get; set; }

    public string? Status { get; set; }

    public DateTime? RequestDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public int? AssignedTo { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Admin? AssignedToNavigation { get; set; }

    public virtual Room? Room { get; set; }

    public virtual Student? Student { get; set; }
}
