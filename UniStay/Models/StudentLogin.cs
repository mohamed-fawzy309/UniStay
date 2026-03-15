using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class StudentLogin
{
    public int LoginId { get; set; }

    public int StudentId { get; set; }

    public string NationalId { get; set; } = null!;

    public byte[]? PasswordHash { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Student Student { get; set; } = null!;
}
