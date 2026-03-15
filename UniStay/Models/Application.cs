using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class Application
{
    public int ApplicationId { get; set; }

    public int StudentId { get; set; }

    public string? AcademicYear { get; set; }

    public string? Semester { get; set; }

    public string HousingPreference { get; set; } = null!;

    public int? PreferredBuildingId { get; set; }

    public int? PreferredRoomId { get; set; }

    public int? PreferredBedNumber { get; set; }

    public string? MealPlanType { get; set; }

    public bool? WithoutNutrition { get; set; }

    public bool? FamilyAbroad { get; set; }

    public bool? SpecialNeeds { get; set; }

    public DateTime? ApplicationDate { get; set; }

    public string? Status { get; set; }

    public int? Priority { get; set; }

    public string? RequiredDocuments { get; set; }

    public string? RejectionReason { get; set; }

    public int? ReviewedBy { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? PaymentDeadline { get; set; }

    public virtual ICollection<Allocation> Allocations { get; set; } = new List<Allocation>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual Building? PreferredBuilding { get; set; }

    public virtual Room? PreferredRoom { get; set; }

    public virtual Admin? ReviewedByNavigation { get; set; }

    public virtual Student Student { get; set; } = null!;
}
