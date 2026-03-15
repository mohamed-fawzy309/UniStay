using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.Data;
using UniStay.Models;
using UniStay.ViewModels;

namespace UniStay.Controllers
{
    public class AdminController : Controller
    {
        private readonly DormitoryDbContext _db;

        public AdminController(DormitoryDbContext db)
        {
            _db = db;
        }

        // ─────────────────────────────────────────────────────────────
        //  GET /Admin/Acceptances
        // ─────────────────────────────────────────────────────────────
        public async Task<IActionResult> Acceptances(string? filter, string? search)
        {
            // Base query: students who have at least one application
            var query = _db.Students
                .AsNoTracking()
                .Where(s => s.IsDeleted != true);

            // Search by name or national ID
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(s =>
                    (s.FullNameArabic != null && s.FullNameArabic.Contains(term)) ||
                    (s.Name != null && s.Name.Contains(term)) ||
                    (s.NationalId != null && s.NationalId.Contains(term)));
            }

            // Count all (before status filter) for the stat cards
            var allStudents = await query
                .Select(s => new { s.Status })
                .ToListAsync();

            int totalCount = allStudents.Count;
            int pendingCount = allStudents.Count(s => s.Status == StudentStatus.Pending);
            int approvedCount = allStudents.Count(s => s.Status == StudentStatus.Approved);
            int rejectedCount = allStudents.Count(s => s.Status == StudentStatus.Rejected);

            // Status filter
            if (!string.IsNullOrWhiteSpace(filter))
            {
                if (Enum.TryParse<StudentStatus>(filter, ignoreCase: true, out var statusFilter))
                    query = query.Where(s => s.Status == statusFilter);
            }

            // Project to ViewModel — grab the most recent application per student
            var students = await query
                .OrderByDescending(s => s.Applications
                    .Where(a => a.IsDeleted != true)
                    .Max(a => (DateTime?)a.ApplicationDate))
                .Select(s => new AcceptanceViewModel
                {
                    StudentId = s.StudentId,
                    Name = s.FullNameArabic ?? s.Name,
                    NationalId = s.NationalId,
                    Faculty = s.Faculty,
                    Grade = s.Grade,
                    StudyType = s.StudyType,
                    Email = s.Email,
                    Mobile = s.Mobile,
                    Phone = s.Phone,
                    StudentCode = s.StudentCode,
                    Gender = s.Gender,
                    Nationality = s.Nationality,
                    HousingType = s.HousingType,
                    BirthDate = s.BirthDate,
                    FatherName = s.FatherName,
                    FatherPhone = s.FatherPhone,
                    SpecialNeeds = s.SpecialNeeds,
                    FamilyAbroad = s.FamilyAbroad,
                    AdminRemarks = s.AdminRemarks,
                    Status = s.Status,

                    // Most recent application fields
                    ApplicationDate = s.Applications
                        .Where(a => a.IsDeleted != true)
                        .OrderByDescending(a => a.ApplicationDate)
                        .Select(a => (DateTime?)a.ApplicationDate)
                        .FirstOrDefault(),
                    AcademicYear = s.Applications
                        .Where(a => a.IsDeleted != true)
                        .OrderByDescending(a => a.ApplicationDate)
                        .Select(a => a.AcademicYear)
                        .FirstOrDefault(),
                    Semester = s.Applications
                        .Where(a => a.IsDeleted != true)
                        .OrderByDescending(a => a.ApplicationDate)
                        .Select(a => a.Semester)
                        .FirstOrDefault(),
                    HousingPreference = s.Applications
                        .Where(a => a.IsDeleted != true)
                        .OrderByDescending(a => a.ApplicationDate)
                        .Select(a => a.HousingPreference)
                        .FirstOrDefault(),
                    MealPlanType = s.Applications
                        .Where(a => a.IsDeleted != true)
                        .OrderByDescending(a => a.ApplicationDate)
                        .Select(a => a.MealPlanType)
                        .FirstOrDefault(),
                })
                .ToListAsync();

            var vm = new AcceptancesPageViewModel
            {
                Students = students,
                TotalCount = totalCount,
                PendingCount = pendingCount,
                ApprovedCount = approvedCount,
                RejectedCount = rejectedCount,
                CurrentFilter = filter,
                SearchTerm = search,
            };

            return View(vm);
        }

        // ─────────────────────────────────────────────────────────────
        //  POST /Admin/Approve
        // ─────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int studentId, string? remarks, string? returnFilter, string? returnSearch)
        {
            var student = await _db.Students.FindAsync(studentId);
            if (student is not null)
            {
                student.Status = StudentStatus.Approved;
                student.ReviewedAt = DateTime.UtcNow;
                if (!string.IsNullOrWhiteSpace(remarks))
                    student.AdminRemarks = remarks;

                // ── Activate the student's login account ──
                var login = await _db.StudentLogins
                    .FirstOrDefaultAsync(l => l.StudentId == studentId && l.IsDeleted != true);
                if (login is not null)
                    login.IsActive = true;

                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Acceptances),
                new { filter = returnFilter, search = returnSearch });
        }

        // ─────────────────────────────────────────────────────────────
        //  POST /Admin/Reject
        // ─────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int studentId, string? remarks, string? returnFilter, string? returnSearch)
        {
            var student = await _db.Students.FindAsync(studentId);
            if (student is not null)
            {
                student.Status = StudentStatus.Rejected;
                student.ReviewedAt = DateTime.UtcNow;
                if (!string.IsNullOrWhiteSpace(remarks))
                    student.AdminRemarks = remarks;
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Acceptances),
                new { filter = returnFilter, search = returnSearch });
        }

        // ─────────────────────────────────────────────────────────────
        //  GET /Admin/Students
        //  Shows all Approved students with their allocation info
        // ─────────────────────────────────────────────────────────────
        public async Task<IActionResult> Students(string? search, string? faculty)
        {
            var query = _db.Students
                .AsNoTracking()
                .Where(s => s.Status == StudentStatus.Approved && s.IsDeleted != true);

            // Search by name or national ID
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(s =>
                    (s.FullNameArabic != null && s.FullNameArabic.Contains(term)) ||
                    (s.Name != null && s.Name.Contains(term)) ||
                    (s.NationalId != null && s.NationalId.Contains(term)) ||
                    (s.StudentCode != null && s.StudentCode.Contains(term)));
            }

            // Faculty filter
            if (!string.IsNullOrWhiteSpace(faculty))
                query = query.Where(s => s.Faculty == faculty);

            var students = await query
                .OrderByDescending(s => s.ReviewedAt)
                .Select(s => new StudentViewModel
                {
                    StudentId = s.StudentId,
                    Name = s.FullNameArabic ?? s.Name,
                    NationalId = s.NationalId,
                    StudentCode = s.StudentCode,
                    Faculty = s.Faculty,
                    Grade = s.Grade,
                    StudyType = s.StudyType,
                    Email = s.Email,
                    Mobile = s.Mobile,
                    Phone = s.Phone,
                    Gender = s.Gender,
                    Nationality = s.Nationality,
                    HousingType = s.HousingType,
                    BirthDate = s.BirthDate,
                    FatherName = s.FatherName,
                    FatherPhone = s.FatherPhone,
                    SpecialNeeds = s.SpecialNeeds,
                    FamilyAbroad = s.FamilyAbroad,
                    AdminRemarks = s.AdminRemarks,
                    ReviewedAt = s.ReviewedAt,
                    ReviewedBy = s.ReviewedBy,

                    // Allocation info if assigned
                    RoomNumber = s.Allocation != null ? s.Allocation.Room.RoomNumber : null,
                    BuildingName = s.Allocation != null ? s.Allocation.Room.Building.BuildingName : null,
                    BedNumber = s.Allocation != null ? (int?)s.Allocation.BedNumber : null,
                    AcademicYear = s.Allocation != null ? s.Allocation.AcademicYear : null,
                    MealPlan = s.Allocation != null ? s.Allocation.MealPlanAllocated : null,
                })
                .ToListAsync();

            // Distinct faculty list for filter dropdown
            var faculties = await _db.Students
                .AsNoTracking()
                .Where(s => s.Status == StudentStatus.Approved && s.IsDeleted != true && s.Faculty != null)
                .Select(s => s.Faculty!)
                .Distinct()
                .OrderBy(f => f)
                .ToListAsync();

            var vm = new StudentsPageViewModel
            {
                Students = students,
                TotalCount = students.Count,
                SearchTerm = search,
                FacultyFilter = faculty,
                Faculties = faculties,
            };

            return View(vm);
        }
    }
}