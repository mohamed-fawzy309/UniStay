using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class StudentDownloadLog
{
    public int LogId { get; set; }

    public int StudentId { get; set; }

    public string? FileName { get; set; }

    public DateTime? DownloadedAt { get; set; }

    public virtual Student Student { get; set; } = null!;
}
