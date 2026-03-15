// Controllers/ApplicationsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using UniStay.Data;
using UniStay.Filters;
using UniStay.Models;

namespace UniStay.Controllers
{
    [AdminAuth, ManagerOrAbove]
    public class ApplicationsController : Controller
    {
        private readonly DormitoryDbContext _db;
        public ApplicationsController(DormitoryDbContext db) => _db = db;

        // ─────────────────────────────────────────
        //  كل الطلبات
        // ─────────────────────────────────────────
        public async Task<IActionResult> Index(string? status, string? year, int page = 1)
        {
            ViewData["Title"] = "إدارة طلبات الإسكان";

            var query = _db.Applications
                .Include(a => a.Student)
                .Include(a => a.PreferredBuilding)
                .Where(a => a.IsDeleted != true)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status)) query = query.Where(a => a.Status == status);
            if (!string.IsNullOrWhiteSpace(year)) query = query.Where(a => a.AcademicYear == year);

            int pageSize = 20;
            int total = await query.CountAsync();

            var apps = await query
                .OrderByDescending(a => a.ApplicationDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Total = total;
            ViewBag.Page = page;
            ViewBag.Status = status;
            ViewBag.Year = year;
            ViewBag.Years = await _db.Applications.Select(a => a.AcademicYear).Distinct().ToListAsync();

            // إحصاء الحالات
            ViewBag.CountPending = await _db.Applications.CountAsync(a => a.Status == "Pending" && a.IsDeleted != true);
            ViewBag.CountApproved = await _db.Applications.CountAsync(a => a.Status == "Approved" && a.IsDeleted != true);
            ViewBag.CountRejected = await _db.Applications.CountAsync(a => a.Status == "Rejected" && a.IsDeleted != true);

            return View(apps);
        }

        // ─────────────────────────────────────────
        //  طلبات معلقة فقط
        // ─────────────────────────────────────────
        public async Task<IActionResult> Pending()
        {
            ViewData["Title"] = "الطلبات المعلقة";
            var apps = await _db.Applications
                .Include(a => a.Student)
                .Include(a => a.PreferredBuilding)
                .Where(a => a.Status == "Pending" && a.IsDeleted != true)
                .OrderBy(a => a.Priority)
                .ThenBy(a => a.ApplicationDate)
                .ToListAsync();
            return View(apps);
        }

        // ─────────────────────────────────────────
        //  تفاصيل طلب
        // ─────────────────────────────────────────
        public async Task<IActionResult> Details(int id)
        {
            ViewData["Title"] = "تفاصيل الطلب";
            var app = await _db.Applications
                .Include(a => a.Student)
                .Include(a => a.PreferredBuilding)
                .Include(a => a.PreferredRoom)
                .Include(a => a.Documents)
                .Include(a => a.ReviewedByNavigation)
                .FirstOrDefaultAsync(a => a.ApplicationId == id && a.IsDeleted != true);

            if (app == null) return NotFound();

            // غرف متاحة لنفس المبنى المطلوب
            ViewBag.AvailableRooms = await _db.Rooms
                .Where(r => r.BuildingId == app.PreferredBuildingId
                         && r.IsActive == true
                         && r.IsDeleted != true
                         && r.CurrentOccupancy < r.BedsCount)
                .ToListAsync();

            return View(app);
        }

        // ─────────────────────────────────────────
        //  قبول طلب + تخصيص غرفة
        // ─────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> Approve(int id, int roomId, int bedNumber)
        {
            var app = await _db.Applications.Include(a => a.Student).FirstOrDefaultAsync(a => a.ApplicationId == id);
            var room = await _db.Rooms.FindAsync(roomId);
            var adminId = int.Parse(HttpContext.Session.GetString("AdminId")!);

            if (app == null || room == null) return NotFound();

            // تحديث الطلب
            app.Status = "Approved";
            app.ReviewedBy = adminId;
            app.ReviewedAt = DateTime.Now;
            app.UpdatedAt = DateTime.Now;

            // إنشاء Allocation
            var alloc = new Allocation
            {
                StudentId = app.StudentId,
                ApplicationId = app.ApplicationId,
                RoomId = roomId,
                BedNumber = bedNumber,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                IsActive = true,
                AllocatedBy = adminId,
                AllocatedAt = DateTime.Now,
                CreatedAt = DateTime.Now
            };
            _db.Allocations.Add(alloc);

            // تحديث إشغال الغرفة
            room.CurrentOccupancy = (room.CurrentOccupancy ?? 0) + 1;

            await _db.SaveChangesAsync();
            TempData["Success"] = $"تم قبول الطلب وتخصيص الغرفة {room.RoomNumber}";
            return RedirectToAction(nameof(Pending));
        }

        // ─────────────────────────────────────────
        //  رفض طلب
        // ─────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> Reject(int id, string reason)
        {
            var app = await _db.Applications.FindAsync(id);
            var adminId = int.Parse(HttpContext.Session.GetString("AdminId")!);

            if (app == null) return NotFound();

            app.Status = "Rejected";
            app.RejectionReason = reason;
            app.ReviewedBy = adminId;
            app.ReviewedAt = DateTime.Now;
            app.UpdatedAt = DateTime.Now;

            await _db.SaveChangesAsync();
            TempData["Error"] = "تم رفض الطلب";
            return RedirectToAction(nameof(Pending));
        }
    }
}
