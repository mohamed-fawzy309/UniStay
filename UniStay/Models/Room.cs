using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public int? Floor { get; set; }

    public string RoomType { get; set; } = null!;

    public int? BedsCount { get; set; }

    public int? CurrentOccupancy { get; set; }

    public int BuildingId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Allocation> Allocations { get; set; } = new List<Allocation>();

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual Building Building { get; set; } = null!;

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
    public decimal PricePerSemester { get; internal set; }
}
