using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace UniStay.Models;

public enum StudentStatus
{
    Pending,
    Approved,
    Rejected
}
public partial class Student
{
    public int StudentId { get; set; }

    public string? StudentCode { get; set; }

    public string StudentStatus { get; set; } = null!;

    public string Nationality { get; set; } = null!;

    public string? Name { get; set; }

    public string? FullNameArabic { get; set; }

    public string? Gender { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? BirthPlace { get; set; }

    public string? Religion { get; set; }

    public int? Residence { get; set; }

    public string? ResidenceCountry { get; set; }

    public int? ResidenceGovernorate { get; set; }

    public int? ResidenceCity { get; set; }

    public string? Address { get; set; }

    public double? DistanceFromHome { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Mobile { get; set; }

    public string? Faculty { get; set; }

    public string? StudyType { get; set; }

    public string? Grade { get; set; }

    public double? GradePercentage { get; set; }

    public string? HousingType { get; set; }

    public string? NationalId { get; set; }

    public string? FatherName { get; set; }

    public string? FatherNationalId { get; set; }

    public string? FatherJob { get; set; }

    public string? FatherPhone { get; set; }

    public string? ParentStatus { get; set; }

    public bool? FamilyAbroad { get; set; }

    public bool? SpecialNeeds { get; set; }

    public string? PassportNumber { get; set; }

    public string? PassportIssuePlace { get; set; }

    public string? HighSchoolDivision { get; set; }

    public double? HighSchoolPercentage { get; set; }

    public double? HighSchoolTotal { get; set; }

    public bool? HighSchoolFromAbroad { get; set; }

    public string? LastYearGrade { get; set; }

    public decimal? LastYearPercentage { get; set; }

    public string? PreviousHousingYears { get; set; }

    public bool? HasMedicalCondition { get; set; }

    public bool? DrugTestPassed { get; set; }

    public string? Notes { get; set; }

    public string? ProfilePhoto { get; set; }

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReviewedAt { get; set; }

    [StringLength(100)]
    public string? ReviewedBy { get; set; }


    public StudentStatus Status { get; set; } = Models.StudentStatus.Pending;
        
    [StringLength(500)]
    [Display(Name = "Admin Remarks")]
    public string? AdminRemarks { get; set; }
    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();

    public virtual Allocation? Allocation { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<EvictionNotice> EvictionNotices { get; set; } = new List<EvictionNotice>();

    public virtual ICollection<Guardian> Guardians { get; set; } = new List<Guardian>();

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<StudentDownloadLog> StudentDownloadLogs { get; set; } = new List<StudentDownloadLog>();

    public virtual StudentLogin? StudentLogin { get; set; }

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
