// Controllers/ReportsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using UniStay.Data;
using UniStay.Filters;

namespace UniStay.Controllers
{
    [AdminAuth, SupervisorOnly]
    public class ReportsController : Controller
    {
        private readonly DormitoryDbContext _db;
        public ReportsController(DormitoryDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "التقارير والإحصائيات";

            var vm = new ReportsViewModel
            {
                // إحصاءات عامة
                TotalStudents = await _db.Students.CountAsync(s => s.IsDeleted != true),
                ActiveStudents = await _db.Students.CountAsync(s => s.StudentStatus == "Active" && s.IsDeleted != true),
                TotalAllocations = await _db.Allocations.CountAsync(a => a.IsActive == true),
                TotalApplications = await _db.Applications.CountAsync(a => a.IsDeleted != true),
                ApprovedApplications = await _db.Applications.CountAsync(a => a.Status == "Approved" && a.IsDeleted != true),
                RejectedApplications = await _db.Applications.CountAsync(a => a.Status == "Rejected" && a.IsDeleted != true),
                TotalRooms = await _db.Rooms.CountAsync(r => r.IsDeleted != true),
                OccupiedRooms = await _db.Rooms.CountAsync(r => r.CurrentOccupancy > 0 && r.IsDeleted != true),

                // توزيع الطلاب بالكلية
                StudentsByFaculty = await _db.Students
                    .Where(s => s.IsDeleted != true && s.Faculty != null)
                    .GroupBy(s => s.Faculty!)
                    .Select(g => new ChartItem { Label = g.Key, Value = g.Count() })
                    .OrderByDescending(x => x.Value)
                    .Take(10)
                    .ToListAsync(),

                // توزيع بالجنسية
                StudentsByNationality = await _db.Students
                    .Where(s => s.IsDeleted != true)
                    .GroupBy(s => s.Nationality)
                    .Select(g => new ChartItem { Label = g.Key, Value = g.Count() })
                    .ToListAsync(),

                // توزيع بالجنس
                StudentsByGender = await _db.Students
                    .Where(s => s.IsDeleted != true && s.Gender != null)
                    .GroupBy(s => s.Gender!)
                    .Select(g => new ChartItem { Label = g.Key, Value = g.Count() })
                    .ToListAsync(),

                // إشغال المباني
                BuildingOccupancy = await _db.Buildings
                    .Include(b => b.Rooms)
                    .Where(b => b.IsDeleted != true)
                    .Select(b => new BuildingOccupancyItem
                    {
                        BuildingName = b.BuildingName,
                        BuildingType = b.BuildingType,
                        TotalBeds = b.Rooms.Sum(r => r.BedsCount ?? 0),
                        OccupiedBeds = b.Rooms.Sum(r => r.CurrentOccupancy ?? 0),
                        TotalRooms = b.Rooms.Count(r => r.IsDeleted != true),
                    })
                    .ToListAsync(),

                // طلبات كل شهر (آخر 6 شهور)
                ApplicationsByMonth = await _db.Applications
                    .Where(a => a.ApplicationDate >= DateTime.Now.AddMonths(-6))
                    .GroupBy(a => new { a.ApplicationDate!.Value.Year, a.ApplicationDate!.Value.Month })
                    .Select(g => new ChartItem
                    {
                        Label = $"{g.Key.Month}/{g.Key.Year}",
                        Value = g.Count()
                    })
                    .ToListAsync(),

                // مخالفات
                TotalViolations = await _db.Violations.CountAsync(v => v.IsDeleted != true),
                // صيانة
                PendingMaintenance = await _db.MaintenanceRequests.CountAsync(m => m.Status == "Pending" && m.IsDeleted != true),
            };

            return View(vm);
        }

        // ─────────────────────────────────────────
        //  تصدير تقرير الطلاب
        // ─────────────────────────────────────────
        public async Task<IActionResult> ExportStudents()
        {
            var data = await _db.Students
                .Where(s => s.IsDeleted != true)
                .Include(s => s.Allocation).ThenInclude(a => a!.Room)
                .OrderBy(s => s.StudentCode)
                .ToListAsync();

            var csv = "كود الطالب,الاسم الكامل,الجنس,الجنسية,الكلية,المرحلة,الحالة,رقم الغرفة,تاريخ التسجيل\n";
            foreach (var s in data)
                csv += $"{s.StudentCode},{s.FullNameArabic},{s.Gender},{s.Nationality},{s.Faculty},{s.Grade},{s.StudentStatus},{s.Allocation?.Room?.RoomNumber ?? "—"},{s.CreatedAt:dd/MM/yyyy}\n";

            return File(System.Text.Encoding.UTF8.GetBytes("\uFEFF" + csv), "text/csv", $"students_report_{DateTime.Now:yyyyMMdd}.csv");
        }

        // ─────────────────────────────────────────
        //  تصدير تقرير الغرف
        // ─────────────────────────────────────────
        public async Task<IActionResult> ExportRooms()
        {
            var data = await _db.Rooms
                .Include(r => r.Building)
                .Where(r => r.IsDeleted != true)
                .OrderBy(r => r.Building.BuildingName).ThenBy(r => r.RoomNumber)
                .ToListAsync();

            var csv = "المبنى,رقم الغرفة,الطابق,النوع,عدد الأسرة,الإشغال الحالي,مكيف,ثلاجة,حمام خاص,الحالة\n";
            foreach (var r in data)
                csv += $"{r.Building.BuildingName},{r.RoomNumber},{r.Floor},{r.RoomType},{r.BedsCount},{r.CurrentOccupancy},{(r.IsActive == true ? "نشطة" : "معطلة")}\n";

            return File(System.Text.Encoding.UTF8.GetBytes("\uFEFF" + csv), "text/csv", $"rooms_report_{DateTime.Now:yyyyMMdd}.csv");
        }
    }

    // ViewModels
    public class ReportsViewModel
    {
        public int TotalStudents { get; set; }
        public int ActiveStudents { get; set; }
        public int TotalAllocations { get; set; }
        public int TotalApplications { get; set; }
        public int ApprovedApplications { get; set; }
        public int RejectedApplications { get; set; }
        public int TotalRooms { get; set; }
        public int OccupiedRooms { get; set; }
        public int TotalViolations { get; set; }
        public int PendingMaintenance { get; set; }

        public List<ChartItem> StudentsByFaculty { get; set; } = [];
        public List<ChartItem> StudentsByNationality { get; set; } = [];
        public List<ChartItem> StudentsByGender { get; set; } = [];
        public List<ChartItem> ApplicationsByMonth { get; set; } = [];
        public List<BuildingOccupancyItem> BuildingOccupancy { get; set; } = [];
    }

    public class ChartItem { public string Label { get; set; } = ""; public int Value { get; set; } }
    public class BuildingOccupancyItem
    {
        public string BuildingName { get; set; } = "";
        public string BuildingType { get; set; } = "";
        public int TotalBeds { get; set; }
        public int OccupiedBeds { get; set; }
        public int TotalRooms { get; set; }
        public double OccupancyPct => TotalBeds > 0 ? Math.Round((double)OccupiedBeds / TotalBeds * 100, 1) : 0;
    }
}
