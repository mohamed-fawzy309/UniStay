using System;
using System.Collections.Generic;

namespace UniStay.Models;

public partial class VwStudentStatus
{
    public int StudentId { get; set; }

    public string? StudentCode { get; set; }

    public string? Name { get; set; }

    public string? FullNameArabic { get; set; }

    public string? NationalId { get; set; }

    public string? PassportNumber { get; set; }

    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? BirthPlace { get; set; }

    public string? Religion { get; set; }

    public string? Residence { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Mobile { get; set; }

    public string? Faculty { get; set; }

    public string? Department { get; set; }

    public string? Grade { get; set; }

    public double? GradePercentage { get; set; }

    public string StudentStatus { get; set; } = null!;

    public string Nationality { get; set; } = null!;

    public string? HousingType { get; set; }

    public double? DistanceFromHome { get; set; }

    public string? FatherName { get; set; }

    public string? FatherPhone { get; set; }

    public string? ParentStatus { get; set; }

    public bool? FamilyAbroad { get; set; }

    public bool? SpecialNeeds { get; set; }

    public string? HighSchoolDivision { get; set; }

    public double? HighSchoolPercentage { get; set; }

    public string? LastYearGrade { get; set; }

    public decimal? LastYearPercentage { get; set; }

    public string? BuildingName { get; set; }

    public string? BuildingType { get; set; }

    public string? RoomNumber { get; set; }

    public int? BedNumber { get; set; }

    public DateOnly? CheckInDate { get; set; }

    public DateOnly? CheckOutDate { get; set; }

    public string? AcademicYear { get; set; }

    public bool? IsCurrentlyResident { get; set; }
}
