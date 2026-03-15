using UniStay.Models;

namespace UniStay.ViewModels
{
    public class AcceptanceViewModel
    {
        public int StudentId { get; set; }
        public string? Name { get; set; }
        public string? NationalId { get; set; }
        public string? Faculty { get; set; }
        public string? Grade { get; set; }
        public string? StudyType { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Phone { get; set; }
        public string? StudentCode { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string? HousingType { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? FatherName { get; set; }
        public string? FatherPhone { get; set; }
        public bool? SpecialNeeds { get; set; }
        public bool? FamilyAbroad { get; set; }
        public string? AdminRemarks { get; set; }
        public StudentStatus Status { get; set; }

        // From Application
        public DateTime? ApplicationDate { get; set; }
        public string? AcademicYear { get; set; }
        public string? Semester { get; set; }
        public string? HousingPreference { get; set; }
        public string? MealPlanType { get; set; }
    }

    public class AcceptancesPageViewModel
    {
        public List<AcceptanceViewModel> Students { get; set; } = new();
        public int TotalCount { get; set; }
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public string? CurrentFilter { get; set; }
        public string? SearchTerm { get; set; }
    }

    // ── Students page ──────────────────────────────────────────────
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        public string? Name { get; set; }
        public string? NationalId { get; set; }
        public string? StudentCode { get; set; }
        public string? Faculty { get; set; }
        public string? Grade { get; set; }
        public string? StudyType { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string? HousingType { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? FatherName { get; set; }
        public string? FatherPhone { get; set; }
        public bool? SpecialNeeds { get; set; }
        public bool? FamilyAbroad { get; set; }
        public string? AdminRemarks { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? ReviewedBy { get; set; }

        // From Allocation → Room → Building
        public string? RoomNumber { get; set; }
        public string? BuildingName { get; set; }
        public int? BedNumber { get; set; }
        public string? AcademicYear { get; set; }
        public string? MealPlan { get; set; }
    }

    public class StudentsPageViewModel
    {
        public List<StudentViewModel> Students { get; set; } = new();
        public int TotalCount { get; set; }
        public string? SearchTerm { get; set; }
        public string? FacultyFilter { get; set; }
        public List<string> Faculties { get; set; } = new();
    }
}