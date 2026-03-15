// Controllers/RoomsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using UniStay.Data;
using UniStay.Filters;
using UniStay.Models;

namespace UniStay.Controllers
{
    [AdminAuth]
    public class RoomsController : Controller
    {
        private readonly DormitoryDbContext _db;
        public RoomsController(DormitoryDbContext db) => _db = db;

        // ─────────────────────────────────────────
        //  قائمة المباني
        // ─────────────────────────────────────────
        public async Task<IActionResult> Buildings()
        {
            ViewData["Title"] = "المباني";
            var buildings = await _db.Buildings
                .Include(b => b.Rooms)
                .Where(b => b.IsDeleted != true)
                .ToListAsync();
            return View(buildings);
        }

        // ─────────────────────────────────────────
        //  قائمة الغرف
        // ─────────────────────────────────────────
        public async Task<IActionResult> Index(int? buildingId, string? type, bool? available)
        {
            ViewData["Title"] = "الغرف";

            var query = _db.Rooms
                .Include(r => r.Building)
                .Where(r => r.IsDeleted != true)
                .AsQueryable();

            if (buildingId.HasValue) query = query.Where(r => r.BuildingId == buildingId);
            if (!string.IsNullOrEmpty(type)) query = query.Where(r => r.RoomType == type);
            if (available == true) query = query.Where(r => r.CurrentOccupancy < r.BedsCount);

            var rooms = await query.OrderBy(r => r.Building.BuildingName).ThenBy(r => r.Floor).ThenBy(r => r.RoomNumber).ToListAsync();

            ViewBag.Buildings = await _db.Buildings.Where(b => b.IsDeleted != true).ToListAsync();
            ViewBag.BuildingId = buildingId;
            ViewBag.Type = type;
            ViewBag.Available = available;

            return View(rooms);
        }

        // ─────────────────────────────────────────
        //  تفاصيل غرفة
        // ─────────────────────────────────────────
        public async Task<IActionResult> Details(int id)
        {
            ViewData["Title"] = "تفاصيل الغرفة";
            var room = await _db.Rooms
                .Include(r => r.Building)
                .Include(r => r.Allocations)
                    .ThenInclude(a => a.Student)
                .Include(r => r.MaintenanceRequests)
                .FirstOrDefaultAsync(r => r.RoomId == id && r.IsDeleted != true);

            if (room == null) return NotFound();
            return View(room);
        }

        // ─────────────────────────────────────────
        //  إضافة غرفة — Manager فوق
        // ─────────────────────────────────────────
        [ManagerOrAbove]
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "إضافة غرفة جديدة";
            ViewBag.Buildings = await _db.Buildings.Where(b => b.IsDeleted != true).ToListAsync();
            return View();
        }

        [HttpPost, ManagerOrAbove, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room model)
        {
            model.CreatedAt = DateTime.Now;
            model.IsActive = true;
            model.IsDeleted = false;
            model.CurrentOccupancy = 0;
            _db.Rooms.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"تم إضافة الغرفة {model.RoomNumber}";
            return RedirectToAction(nameof(Index));
        }

        // ─────────────────────────────────────────
        //  تعديل غرفة
        // ─────────────────────────────────────────
        [ManagerOrAbove]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "تعديل الغرفة";
            var room = await _db.Rooms.FindAsync(id);
            if (room == null) return NotFound();
            ViewBag.Buildings = await _db.Buildings.Where(b => b.IsDeleted != true).ToListAsync();
            return View(room);
        }

        [HttpPost, ManagerOrAbove, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Room model)
        {
            var room = await _db.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            room.RoomNumber = model.RoomNumber;
            room.Floor = model.Floor;
            room.RoomType = model.RoomType;
            room.BedsCount = model.BedsCount;
            room.BuildingId = model.BuildingId;
            room.IsActive = model.IsActive;

            await _db.SaveChangesAsync();
            TempData["Success"] = "تم تحديث بيانات الغرفة";
            return RedirectToAction(nameof(Index));
        }

        // ─────────────────────────────────────────
        //  تفعيل / تعطيل غرفة
        // ─────────────────────────────────────────
        [HttpPost, ManagerOrAbove]
        public async Task<IActionResult> Toggle(int id)
        {
            var room = await _db.Rooms.FindAsync(id);
            if (room == null) return NotFound();
            room.IsActive = !room.IsActive;
            await _db.SaveChangesAsync();
            TempData["Success"] = "تم تحديث حالة الغرفة";
            return RedirectToAction(nameof(Index));
        }
    }
}
