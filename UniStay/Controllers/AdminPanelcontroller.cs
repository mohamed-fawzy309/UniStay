// ============================================================
// Controllers/AdminPanelController.cs  — FINAL VERSION
// ============================================================

using DormitorySystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UniStay.Data;
using UniStay.Filters;
using UniStay.Models;
using UniStay.ViewModels;

namespace UniStay.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPanelController : Controller
    {
        private readonly DormitoryDbContext _db;

        public AdminPanelController(DormitoryDbContext db) => _db = db;

        // ── Current admin id from cookie claims ──────────────────────────────
        private int? CurrentAdminId =>
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
                ? id : null;

        // ====================================================================
        // DASHBOARD
        // ====================================================================

        public async Task<IActionResult> Dashboard()
        {
            var vm = new DashboardViewModel
            {
                // Student counts
                TotalStudents = await _db.Students.CountAsync(s => s.IsDeleted != true),
                PendingCount = await _db.Students.CountAsync(s => s.Status == StudentStatus.Pending && s.IsDeleted != true),
                ApprovedCount = await _db.AcceptingStudents.CountAsync(s => s.Status == StudentStatus.Approved && s.IsDeleted != true),
                RejectedCount = await _db.Students.CountAsync(s => s.Status == StudentStatus.Rejected && s.IsDeleted != true),
                MaleCount = await _db.AcceptingStudents.CountAsync(s => s.Gender == "Male" && s.IsDeleted != true),
                FemaleCount = await _db.AcceptingStudents.CountAsync(s => s.Gender == "Female" && s.IsDeleted != true),

                // Application counts
                PendingApplications = await _db.Applications.CountAsync(a => a.Status == "Pending" && a.IsDeleted != true),
                TotalAccepted = await _db.Applications.CountAsync(a => a.Status == "Accepted" && a.IsDeleted != true),
                TotalRejected = await _db.Applications.CountAsync(a => a.Status == "Rejected" && a.IsDeleted != true),

                // Room & allocation counts
                ActiveAllocations = await _db.Allocations.CountAsync(a => a.IsActive == true),
                TotalRooms = await _db.Rooms.CountAsync(r => r.IsDeleted != true && r.IsActive == true),
                OccupiedRooms = await _db.Rooms.CountAsync(r => r.CurrentOccupancy > 0 && r.IsActive == true),

                // Other counts
                TodayMeals = await _db.Meals.CountAsync(m => m.MealDate == DateOnly.FromDateTime(DateTime.Today) && m.IsBooked == true),
                PendingMaintenance = await _db.MaintenanceRequests.CountAsync(m => m.Status == "Pending" && m.IsDeleted != true),
                ActiveViolations = await _db.Violations.CountAsync(v => v.IsDeleted != true),
                TotalAdmins = await _db.Admins.CountAsync(a => a.IsActive == true && a.IsDeleted != true),

                // Recent lists
                RecentPending = await _db.Students
                    .Where(s => s.Status == StudentStatus.Pending && s.IsDeleted != true)
                    .OrderByDescending(s => s.RegisteredAt)
                    .Take(5)
                    .ToListAsync(),

                RecentApplications = await _db.Applications
                    .Include(a => a.Student)
                    .Where(a => a.IsDeleted != true)
                    .OrderByDescending(a => a.ApplicationDate)
                    .Take(8)
                    .ToListAsync(),

                RegistrationDates = await _db.RegistrationDates
                    .OrderBy(d => d.StartDate)
                    .ToListAsync(),

                BuildingStats = await _db.Buildings
                    .Include(b => b.Rooms)
                    .Where(b => b.IsDeleted != true)
                    .Select(b => new BuildingStat
                    {
                        Name = b.BuildingName,
                        TotalBeds = b.Rooms.Sum(r => r.BedsCount ?? 0),
                        OccupiedBeds = b.Rooms.Sum(r => r.CurrentOccupancy ?? 0),
                    })
                    .ToListAsync()
            };

            ViewData["Title"] = "لوحة التحكم";
            ViewData["Layout"] = "_AdminLayout";
            return View(vm);
        }

        // ====================================================================
        // SIDEBAR COUNTS  (AJAX)
        // ====================================================================

        [HttpGet]
        public async Task<IActionResult> SidebarCounts()
        {
            return Json(new
            {
                AcceptingStudents = await _db.AcceptingStudents.CountAsync(s => s.IsDeleted != true),
                pending = await _db.Applications.CountAsync(a => a.Status == "Pending" && a.IsDeleted != true),
            });
        }

        // ====================================================================
        // UPDATE APPLICATION STATUS  (AJAX — called from dashboard)
        // ====================================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAcceptanceStatus([FromBody] UpdateStatusRequest req)
        {
            var app = await _db.Applications.FindAsync(req.Id);
            if (app == null) return NotFound();

            app.Status = req.Status;
            app.ReviewedBy = CurrentAdminId;
            app.ReviewedAt = DateTime.UtcNow;
            app.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok();
        }

        // DTO used by UpdateAcceptanceStatus
        public class UpdateStatusRequest
        {
            public int Id { get; set; }
            public string Status { get; set; } = "";
        }

        // ====================================================================
        // PENDING STUDENT REGISTRATIONS
        // ====================================================================

        public async Task<IActionResult> Pending()
        {
            var list = await _db.AcceptingStudents
                .Where(s => s.Status == StudentStatus.Pending && s.IsDeleted != true)
                .OrderBy(s => s.RegisteredAt)
                .ToListAsync();

            return View(list);
        }

        // ====================================================================
        // ALL AcceptingStudents  (search + status + nationality + pagination)
        // ====================================================================

        public async Task<IActionResult> AcceptingStudents(
            string? search,
            string? statusFilter,
            string? nationality,
            int page = 1)
        {
            const int pageSize = 20;

            var query = _db.AcceptingStudents
                .Where(s => s.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrEmpty(statusFilter) &&
                Enum.TryParse<StudentStatus>(statusFilter, out var parsed))
                query = query.Where(s => s.Status == parsed);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(s =>
                    (s.FullNameArabic != null && s.FullNameArabic.Contains(search)) ||
                    (s.NationalId != null && s.NationalId.Contains(search)) ||
                    (s.StudentCode != null && s.StudentCode.Contains(search)) ||
                    (s.Email != null && s.Email.Contains(search)));

            if (!string.IsNullOrEmpty(nationality))
                query = query.Where(s => s.Nationality == nationality);

            ViewBag.TotalCount = await query.CountAsync();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);
            ViewBag.Search = search;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.Nationality = nationality;

            var AcceptingStudents = await query
                .OrderByDescending(s => s.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(AcceptingStudents);
        }

        // ====================================================================
        // STUDENT DETAIL
        // ====================================================================

        public async Task<IActionResult> StudentDetails(int id)
        {
            var student = await _db.AcceptingStudents
                .Include(s => s.Guardians)
                .Include(s => s.Applications)
                .Include(s => s.Allocation)
                    .ThenInclude(a => a!.Room)
                        .ThenInclude(r => r!.Building)
                .Include(s => s.Payments)
                .FirstOrDefaultAsync(s => s.AcceptingStudentId == id && s.IsDeleted == false);

            if (student == null) return NotFound();
            return View(student);
        }

        // Alias — keeps /Admin/Detail/5 working
        public Task<IActionResult> Detail(int id) => StudentDetails(id);

        // ====================================================================
        // APPROVE / REJECT STUDENT REGISTRATION
        // ====================================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(ApprovalActionViewModel model)
        {
            var student = await _db.AcceptingStudents.FindAsync(model.StudentId);
            if (student == null)
            {
                TempData["Error"] = "الطالب غير موجود.";
                return RedirectToAction(nameof(Pending));
            }

            student.Status = StudentStatus.Approved;
            student.ReviewedAt = DateTime.UtcNow;
            student.ReviewedBy = User.Identity?.Name;
            student.AdminRemarks = model.Remarks?.Trim();

            await _db.SaveChangesAsync();
            TempData["Success"] = $"تم قبول تسجيل {student.FullNameArabic ?? student.Name} بنجاح.";
            return RedirectToAction(nameof(Pending));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(ApprovalActionViewModel model)
        {
            var student = await _db.AcceptingStudents.FindAsync(model.StudentId);
            if (student == null)
            {
                TempData["Error"] = "الطالب غير موجود.";
                return RedirectToAction(nameof(Pending));
            }

            student.Status = StudentStatus.Rejected;
            student.ReviewedAt = DateTime.UtcNow;
            student.ReviewedBy = User.Identity?.Name;
            student.AdminRemarks = model.Remarks?.Trim();

            await _db.SaveChangesAsync();
            TempData["Success"] = $"تم رفض تسجيل {student.FullNameArabic ?? student.Name}.";
            return RedirectToAction(nameof(Pending));
        }

        // ====================================================================
        // APPLICATIONS
        // ====================================================================

        public async Task<IActionResult> Applications(string? status, int page = 1)
        {
            const int pageSize = 20;

            var query = _db.Applications
                .Include(a => a.Student)
                .Where(a => a.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(a => a.Status == status);

            ViewBag.Status = status;
            ViewBag.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);
            ViewBag.Page = page;

            var apps = await query
                .OrderByDescending(a => a.ApplicationDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(apps);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateApplicationStatus(
            int applicationId,
            string newStatus,
            string? rejectionReason)
        {
            var app = await _db.Applications.FindAsync(applicationId);
            if (app == null) return NotFound();

            app.Status = newStatus;
            app.RejectionReason = rejectionReason;
            app.ReviewedBy = CurrentAdminId;
            app.ReviewedAt = DateTime.UtcNow;
            app.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            TempData["Success"] = "تم تحديث حالة الطلب.";
            return RedirectToAction(nameof(Applications));
        }

        // ====================================================================
        // ROOMS
        // ====================================================================

        public async Task<IActionResult> Rooms(int? buildingId)
        {
            var query = _db.Rooms
                .Include(r => r.Building)
                .Where(r => r.IsDeleted == false && r.IsActive == true)
                .AsQueryable();

            if (buildingId.HasValue)
                query = query.Where(r => r.BuildingId == buildingId);

            ViewBag.Buildings = await _db.Buildings.Where(b => b.IsDeleted == false).ToListAsync();
            ViewBag.SelectedBuilding = buildingId;

            var rooms = await query
                .OrderBy(r => r.BuildingId)
                .ThenBy(r => r.Floor)
                .ThenBy(r => r.RoomNumber)
                .ToListAsync();

            return View(rooms);
        }

        // ====================================================================
        // ALLOCATE
        // ====================================================================

        [HttpGet]
        public async Task<IActionResult> Allocate(int applicationId)
        {
            var app = await _db.Applications
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

            if (app == null) return NotFound();

            var rooms = await _db.Rooms
                .Include(r => r.Building)
                .Where(r =>
                    r.IsDeleted == false &&
                    r.IsActive == true &&
                    r.CurrentOccupancy < r.BedsCount &&
                    r.RoomType == app.HousingPreference)
                .ToListAsync();

            ViewBag.Application = app;
            ViewBag.AvailableRooms = rooms;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Allocate(int applicationId, int roomId, int bedNumber)
        {
            var app = await _db.Applications.FindAsync(applicationId);
            var room = await _db.Rooms.FindAsync(roomId);

            if (app == null || room == null) return NotFound();

            _db.Allocations.Add(new Allocation
            {
                ApplicationId = applicationId,
                StudentId = app.StudentId,
                RoomId = roomId,
                BedNumber = bedNumber,
                AcademicYear = app.AcademicYear,
                Semester = app.Semester,
                FromDate = DateOnly.FromDateTime(DateTime.Today),
                IsActive = true,
                AllocatedBy = CurrentAdminId,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            });

            room.CurrentOccupancy++;
            app.Status = "Accepted";
            app.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            TempData["Success"] = "تم تخصيص الغرفة بنجاح.";
            return RedirectToAction(nameof(Applications));
        }

        // ====================================================================
        // ANNOUNCEMENTS
        // ====================================================================

        public async Task<IActionResult> Announcements()
        {
            var list = await _db.Announcements
                .Where(a => a.IsDeleted == false)
                .OrderByDescending(a => a.PublishedAt)
                .ToListAsync();

            return View(list);
        }

        [HttpGet]
        public IActionResult CreateAnnouncement() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnnouncement(
            string title,
            string content,
            string? academicYear,
            bool isPublic)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "العنوان والمحتوى مطلوبان.";
                return View();
            }

            _db.Announcements.Add(new Announcement
            {
                Title = title.Trim(),
                Content = content.Trim(),
                AcademicYear = academicYear,
                IsPublic = isPublic,
                CreatedBy = CurrentAdminId,
                PublishedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            });

            await _db.SaveChangesAsync();
            TempData["Success"] = "تم نشر الإعلان بنجاح.";
            return RedirectToAction(nameof(Announcements));
        }

        // ====================================================================
        // REGISTRATION DATES
        // ====================================================================

        public async Task<IActionResult> Dates()
        {
            var dates = await _db.RegistrationDates
                .OrderBy(d => d.StartDate)
                .ToListAsync();
            return View(dates);
        }

        [HttpGet]
        public IActionResult DateCreate() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DateCreate(RegistrationDate model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.RegistrationDates.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "تم إضافة الموعد بنجاح.";
            return RedirectToAction(nameof(Dates));
        }

        [HttpGet]
        public async Task<IActionResult> DateEdit(int id)
        {
            var date = await _db.RegistrationDates.FindAsync(id);
            if (date == null) return NotFound();
            return View(date);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DateEdit(RegistrationDate model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.RegistrationDates.Update(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "تم تحديث الموعد بنجاح.";
            return RedirectToAction(nameof(Dates));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DateDelete(int id)
        {
            var date = await _db.RegistrationDates.FindAsync(id);
            if (date == null) return NotFound();
            _db.RegistrationDates.Remove(date);
            await _db.SaveChangesAsync();
            TempData["Success"] = "تم حذف الموعد بنجاح.";
            return RedirectToAction(nameof(Dates));
        }

        // ====================================================================
        // ADMIN / STAFF MANAGEMENT  (SupervisorOnly)
        // ====================================================================

        [SupervisorOnly]
        public async Task<IActionResult> Admins()
        {
            ViewData["Title"] = "إدارة المسؤولين";
            var admins = await _db.Admins
                .Where(a => a.IsDeleted != true)
                .OrderBy(a => a.Role)
                .ToListAsync();
            return View(admins);
        }

        // Called from the Add Staff modal on the dashboard
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStaff(
            string name,
            string username,
            string email,
            string? phone,
            string role,
            string password)
        {
            // Check duplicate username
            if (await _db.Admins.AnyAsync(a => a.Username == username && a.IsDeleted != true))
            {
                TempData["Error"] = "اسم المستخدم مستخدم بالفعل.";
                return RedirectToAction(nameof(Index));
            }

            _db.Admins.Add(new Admin
            {
                Name = name.Trim(),
                Username = username.Trim(),
                Email = email.Trim(),
                Phone = phone?.Trim(),
                Role = role,
                PasswordHash = SHA256.HashData(Encoding.UTF8.GetBytes(password)),
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
            });

            await _db.SaveChangesAsync();
            TempData["Success"] = $"تم إضافة الموظف {name} بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [SupervisorOnly]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAdmin(int id)
        {
            var admin = await _db.Admins.FindAsync(id);
            if (admin == null) return NotFound();

            admin.IsActive = !(admin.IsActive ?? false);
            await _db.SaveChangesAsync();
            TempData["Success"] = "تم تحديث حالة المسؤول.";
            return RedirectToAction(nameof(Admins));
        }

        [HttpPost]
        [SupervisorOnly]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _db.Admins.FindAsync(id);
            if (admin == null) return NotFound();

            admin.IsDeleted = true;
            await _db.SaveChangesAsync();
            TempData["Success"] = "تم حذف المسؤول.";
            return RedirectToAction(nameof(Admins));
        }
    }

    // ========================================================================
    // DASHBOARD VIEW MODEL
    // ========================================================================

    public class DashboardViewModel
    {
        // Student stats
        public int TotalStudents { get; set; }
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public int MaleCount { get; set; }
        public int FemaleCount { get; set; }

        // Application stats
        public int PendingApplications { get; set; }
        public int TotalAccepted { get; set; }
        public int TotalRejected { get; set; }

        // Room & allocation stats
        public int ActiveAllocations { get; set; }
        public int TotalRooms { get; set; }
        public int OccupiedRooms { get; set; }

        // Other stats
        public int TodayMeals { get; set; }
        public int PendingMaintenance { get; set; }
        public int ActiveViolations { get; set; }
        public int TotalAdmins { get; set; }

        // Lists
        public List<Student> RecentPending { get; set; } = new();
        public List<Application> RecentApplications { get; set; } = new();
        public List<RegistrationDate> RegistrationDates { get; set; } = new();
        public List<BuildingStat> BuildingStats { get; set; } = new();
    }

    public class BuildingStat
    {
        public string Name { get; set; } = "";
        public int TotalBeds { get; set; }
        public int OccupiedBeds { get; set; }
        public double OccupancyPct => TotalBeds > 0
            ? Math.Round((double)OccupiedBeds / TotalBeds * 100, 1) : 0;
    }
}