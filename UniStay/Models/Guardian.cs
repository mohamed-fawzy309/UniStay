using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class Guardian
{
    public int GuardianId { get; set; }

    public int StudentId { get; set; }

    public string? GuardianRole { get; set; }

    public string? Name { get; set; }

    public string? NationalId { get; set; }

    public string? Relation { get; set; }

    public string? Phone { get; set; }

    public string? AlternatePhone { get; set; }

    public string? Job { get; set; }

    public string? ResidenceAddress { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Student Student { get; set; } = null!;
}
