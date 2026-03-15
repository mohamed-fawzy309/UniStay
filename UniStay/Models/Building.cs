using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class Building
{
    public int BuildingId { get; set; }

    public string BuildingName { get; set; } = null!;

    public string BuildingType { get; set; } = null!;

    public int? FloorsCount { get; set; }

    public int? TotalRooms { get; set; }

    public int DormitoryId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual Dormitory Dormitory { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
    public bool IsActive { get; internal set; }
}
