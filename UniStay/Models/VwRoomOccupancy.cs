using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class VwRoomOccupancy
{
    public string BuildingName { get; set; } = null!;

    public string BuildingType { get; set; } = null!;

    public int RoomId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public int? Floor { get; set; }

    public string RoomType { get; set; } = null!;

    public int? BedsCount { get; set; }

    public int? CurrentOccupancy { get; set; }

    public int? AvailableBeds { get; set; }

    public string OccupancyStatus { get; set; } = null!;
}
