using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class AnnouncementAttachment
{
    public int AttachmentId { get; set; }

    public int AnnouncementId { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public string? FileType { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Announcement Announcement { get; set; } = null!;
}
