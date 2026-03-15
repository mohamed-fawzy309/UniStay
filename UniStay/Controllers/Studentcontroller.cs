using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UniStay.Data;
using UniStay.Models;
using UniStay.ViewModels;

namespace UniStay.Controllers
{
    public class StudentController : Controller
    {
        private readonly DormitoryDbContext _db;

        public StudentController(DormitoryDbContext db)
        {
            _db = db;
        }

        // ───────────── Auth Helpers ─────────────

        private int? CurrentStudentID =>
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id : null;

        private IActionResult? CheckAuth()
        {
            if (CurrentStudentID == null)
                return RedirectToAction("StudentLogin", "Auth");

            return null;
        }

        // ───────────────── Dashboard ─────────────────

        public async Task<IActionResult> Dashboard()
        {
            var check = CheckAuth();
            if (check != null) return check;

            var student = await _db.Students
                .Include(s => s.Allocation)
                    .ThenInclude(a => a!.Room)
                        .ThenInclude(r => r!.Building)
                .Include(s => s.Applications)
                .FirstOrDefaultAsync(s =>
                    s.StudentId == CurrentStudentID &&
                    s.IsDeleted == false);

            return View(student);
        }

        // ───────────────── Profile ─────────────────

        public async Task<IActionResult> Profile()
        {
            var check = CheckAuth();
            if (check != null) return check;

            var student = await _db.Students
                .Include(s => s.Guardians)
                .FirstOrDefaultAsync(s =>
                    s.StudentId == CurrentStudentID &&
                    s.IsDeleted == false);

            return View(student);
        }

        // ───────────────── Apply (GET) ─────────────────

        [HttpGet]
        public async Task<IActionResult> Apply()
        {
            var check = CheckAuth();
            if (check != null) return check;

            var existing = await _db.Applications
                .Where(a => a.StudentId == CurrentStudentID &&
                           (a.Status == "PendingPayment" || a.Status == "Accepted") &&
                            a.IsDeleted != true)
                .FirstOrDefaultAsync();

            if (existing != null)
            {
                TempData["Error"] = existing.Status == "PendingPayment"
                    ? "لديك حجز قيد الدفع. يرجى إتمام الدفع أولاً."
                    : "أنت مقيم حالياً ولا يمكنك تقديم طلب جديد.";

                return RedirectToAction("Dashboard");
            }

            ViewBag.Buildings = await _db.Buildings
                .Where(b => b.IsActive && b.IsDeleted != true)
                .OrderBy(b => b.BuildingName)
                .ToListAsync();

            return View(new ApplyViewModel
            {
                AcademicYear = $"{DateTime.Now.Year}-{DateTime.Now.Year + 1}"
            });
        }

        // ───────────────── Apply (POST) ─────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(ApplyViewModel model)
        {
            var check = CheckAuth();
            if (check != null) return check;

            if (!ModelState.IsValid)
            {
                ViewBag.Buildings = await _db.Buildings
                    .Where(b => b.IsActive && b.IsDeleted != true)
                    .ToListAsync();

                return View(model);
            }


            var room = await _db.Rooms
                    .FirstOrDefaultAsync(r =>
                        r.RoomId == model.RoomId &&
                        r.BuildingId == model.BuildingId &&
                        r.CurrentOccupancy < r.BedsCount &&
                        r.IsActive == true &&
                        r.IsDeleted != true);

            if (room == null)
            {
                TempData["Error"] = "هذه الغرفة غير متاحة حالياً.";
                return RedirectToAction("Apply");
            }

            var takenBeds = await _db.Allocations
                .Where(a => a.RoomId == model.RoomId && a.Status != "Cancelled")
                .Select(a => a.BedNumber)
                .ToListAsync();

            int bedNumber = 1;
            for (int i = 1; i <= (room.BedsCount ?? 1); i++)
            {
                if (!takenBeds.Contains(i))
                {
                    bedNumber = i;
                    break;
                }
            }

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var application = new Application
                {
                    StudentId = CurrentStudentID!.Value,
                    PreferredRoomId = model.RoomId,
                    AcademicYear = model.AcademicYear,
                    HousingPreference = model.HousingPreference ?? "",
                    Status = "PendingPayment",
                    CreatedAt = DateTime.Now,
                    PaymentDeadline = DateTime.Now.AddDays(1),
                    IsDeleted = false
                };

                _db.Applications.Add(application);
                await _db.SaveChangesAsync(); // مهم جدا عشان يتولد ApplicationID

                var allocation = new Allocation
                {
                    StudentId = CurrentStudentID.Value,
                    RoomId = model.RoomId,
                    ApplicationId = application.ApplicationId, // هنا الحل
                    BedNumber = bedNumber,
                    FromDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                    Status = "Reserved"
                };

                _db.Allocations.Add(allocation);

                room.CurrentOccupancy = (room.CurrentOccupancy ?? 0) + 1;

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = "تم حجز الغرفة بنجاح! لديك 24 ساعة لإتمام الدفع.";

                return RedirectToAction("Payment");
            }
            catch
            {
                await transaction.RollbackAsync();

                TempData["Error"] =
                    "حدث خطأ أثناء الحجز.";

                return RedirectToAction("Apply");
            }
        }

        // ───────────────── Payments Page ─────────────────

        public async Task<IActionResult> Payment()
        {
            var check = CheckAuth();
            if (check != null) return check;

            var payments = await _db.Payments
                .Where(p => p.StudentId == CurrentStudentID && p.IsDeleted == false)
                .OrderByDescending(p => p.PaymentDate ?? p.CreatedAt)
                .ToListAsync();

            var pendingApp = await _db.Applications
                .Include(a => a.PreferredRoom)
                .ThenInclude(r => r!.Building)
                .FirstOrDefaultAsync(a =>
                    a.StudentId == CurrentStudentID &&
                    a.Status == "PendingPayment" &&
                    a.IsDeleted == false);

            var vm = new PaymentsViewModel
            {
                PendingApplication = pendingApp,
                PendingPayments = payments.Where(p => p.PaymentDate == null).ToList(),
                OverduePayments = payments.Where(p => p.IsOverdue == true).ToList(),
                AllPayments = payments,
                TotalPaid = payments
                    .Where(p => p.PaymentDate != null)
                    .Sum(p => p.Amount ?? 0)
            };

            return View(vm);
        }

        // ───────────────── Confirm Payment (Simulation) ─────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(
            int applicationId,
            string paymentMethod,
            decimal amount)
        {
            var check = CheckAuth();
            if (check != null) return check;

            var application = await _db.Applications
                .FirstOrDefaultAsync(a =>
                    a.ApplicationId == applicationId &&
                    a.StudentId == CurrentStudentID &&
                    a.Status == "PendingPayment");

            if (application == null)
            {
                TempData["Error"] = "الطلب غير موجود.";
                return RedirectToAction("Payments");
            }

            using var tx = await _db.Database.BeginTransactionAsync();

            try
            {
                var alloc = await _db.Allocations
                    .FirstOrDefaultAsync(a =>
                        a.StudentId == CurrentStudentID &&
                        a.RoomId == application.PreferredRoomId &&
                        a.Status == "Reserved");

                if (alloc != null)
                    alloc.Status = "Active";

                var payment = new Payment
                {
                    StudentId = CurrentStudentID!.Value,
                    AllocationId = alloc?.AllocationId,
                    AcademicYear = application.AcademicYear,
                    Semester = "الأول",
                    PaymentCategory = "Housing",
                    Amount = amount,
                    PaymentMethod = paymentMethod,
                    PaymentType = "Full",
                    PaymentDate = DateTime.Now,
                    ReceiptNumber = $"RCP-{DateTime.Now.Ticks % 1000000}",
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                _db.Payments.Add(payment);

                application.Status = "Accepted";

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                TempData["Success"] =
                    $"تم الدفع بنجاح. رقم الإيصال: {payment.ReceiptNumber}";

                return RedirectToAction("Dashboard");
            }
            catch
            {
                await tx.RollbackAsync();

                TempData["Error"] = "فشل الدفع.";

                return RedirectToAction("Payments");
            }
        }

        // ───────────────── AJAX Get Rooms ─────────────────

        [HttpGet]
        public async Task<IActionResult> GetRooms(int buildingId)
        {
            var rooms = await _db.Rooms
                .Where(r => 
                    r.BuildingId == buildingId &&
                    r.IsActive == true &&
                    r.IsDeleted != true)
                .Select(r => new
                {
                    roomId = r.RoomId,
                    roomNumber = r.RoomNumber,
                    bedsCount = r.BedsCount,
                    currentOccupancy = r.CurrentOccupancy,
                    freeBeds = (r.BedsCount ?? 0) - (r.CurrentOccupancy ?? 0)
                })
                .ToListAsync();

            return Json(rooms);
        }
    }
}
