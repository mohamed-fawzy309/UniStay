// ============================================================
// Controllers/AuthController.cs
// Merged from DormitorySystem (V1) + UniStay (V2)
//
// Key decisions:
//   - Cookie-based auth for BOTH Student and Admin (V1 approach,
//     more secure than plain sessions).
//   - Password stored as byte[] SHA-256 (V2 approach).
//   - Registration uses the richer V2 Student model
//     (NationalId, Guardian, StudentLogin entity, etc.).
//   - Student login accepts NationalId  (V2) OR Email (V1) —
//     whichever is present in the submitted form.
//   - Admin login kept from V2.
//   - AccessDenied action kept from V1.
//
// ⚠  Adjust the DbContext name / namespace imports to match
//    whichever project this file lives in.
// ============================================================

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

// ── Replace these with the correct namespaces for your project ──────────────
using UniStay.Data;          // ApplicationDbContext  /  DormitoryDbContext
using UniStay.Models;        // Student, Admin, Guardian, StudentLogin, …
using UniStay.ViewModels;    // StudentLoginViewModel, AdminLoginViewModel,
                                 // StudentRegisterViewModel, LoginViewModel
                                 // ────────────────────────────────────────────────────────────────────────────

namespace YourProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly DormitoryDbContext _db;

        public AuthController(DormitoryDbContext db)
        {
            _db = db;
        }

        // ====================================================================
        // STUDENT REGISTRATION
        // Merged: V1 duplicate-checks (email + studentNumber) +
        //         V2 rich model (nationality, guardian, StudentLogin entity)
        // ====================================================================

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Dashboard", "Student");

            return View(new StudentRegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(StudentRegisterViewModel model)
        {
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                

            // ── Duplicate NationalId check (V2) ─────────────────────────────
            if (model.Nationality == "Egyptian" && !string.IsNullOrEmpty(model.NationalID))
            {
                bool nationalIdExists = await _db.Students
                    .AnyAsync(s => s.NationalId == model.NationalID && s.IsDeleted == false);

                if (nationalIdExists)
                {
                    ModelState.AddModelError("NationalID", "الرقم القومي مسجل بالفعل.");
                    return View(model);
                }
            }

            // ── Duplicate email check (V1) ───────────────────────────────────
            if (!string.IsNullOrEmpty(model.Email))
            {
                bool emailExists = await _db.Students
                    .AnyAsync(s => s.Email!.ToLower() == model.Email.ToLower()
                                   && s.IsDeleted == false);

                if (emailExists)
                {
                    ModelState.AddModelError("Email", "An account with this email already exists.");
                    return View(model);
                }
            }

            // ── Create Student (V2 rich model) ───────────────────────────────
            var student = new Student
            {
                // ---- identity / personal ----
                Nationality = model.Nationality,
                FullNameArabic = model.FullNameArabic,
                Gender = model.Gender,
                BirthDate = model.BirthDate,
                BirthPlace = model.BirthPlace,
                Religion = model.Religion,
                // ---- contact ----
                Email = model.Email?.Trim().ToLower(),
                Mobile = model.Mobile,
                // ---- address ----
                Residence = model.CityId,
                ResidenceGovernorate = model.GovernorateId,
                ResidenceCity = model.CenterId,
                Address = model.Address,
                // ---- academic ----
                StudentStatus = model.StudentStatus,
                Faculty = model.Faculty,
                Grade = model.Grade,
                HousingType = model.HousingType,
                // ---- Egyptian-specific ----
                NationalId = model.NationalID,
                FatherName = model.FatherName,
                FatherNationalId = model.FatherNationalID,
                FatherJob = model.FatherJob,
                FatherPhone = model.FatherPhone,
                ParentStatus = model.ParentStatus,
                FamilyAbroad = model.FamilyAbroad,
                SpecialNeeds = model.SpecialNeeds,
                // ---- non-Egyptian ----
                PassportNumber = model.PassportNumber,
                PassportIssuePlace = model.PassportIssuePlace,
                // ---- new student ----
                HighSchoolDivision = model.HighSchoolDivision,
                HighSchoolTotal = model.HighSchoolTotal,
                HighSchoolPercentage = model.HighSchoolPercentage,
                HighSchoolFromAbroad = model.HighSchoolFromAbroad,
                // ---- returning student ----
                LastYearGrade = model.LastYearGrade,
                LastYearPercentage = model.LastYearPercentage,
                // ---- system ----
                Status = StudentStatus.Pending,   // V1: always Pending
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            _db.Students.Add(student);
            await _db.SaveChangesAsync();   // save first so we have StudentId

            // ── Guardian (V2) ────────────────────────────────────────────────
            if (!string.IsNullOrEmpty(model.GuardianName))
            {
                _db.Guardians.Add(new Guardian
                {
                    StudentId = student.StudentId,
                    Name = model.GuardianName,
                    Relation = model.GuardianRelation,
                    NationalId = model.GuardianNationalID,
                    Phone = model.GuardianPhone,
                    GuardianRole = "Other",
                    IsDeleted = false
                });
            }

            // ── StudentLogin record (V2) ─────────────────────────────────────
            _db.StudentLogins.Add(new StudentLogin
            {
                StudentId = student.StudentId,
                NationalId = model.NationalID ?? model.PassportNumber ?? "",
                PasswordHash = HashPassword(model.Password),
                IsActive = false,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            });

            await _db.SaveChangesAsync();

            TempData["RegistrationSuccess"] =
                "تم إرسال طلبك بنجاح وسيتم مراجعته من قبل الإدارة.";

            return RedirectToAction("Index", "Home");
        }

        // ====================================================================
        // STUDENT LOGIN
        // Merged: V1 cookie auth + status gate +
        //         V2 NationalId lookup via StudentLogin entity
        // ====================================================================

        [HttpGet]
        public IActionResult StudentLogin(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Dashboard", "Student");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentLogin(StudentLoginViewModel model,
                                                       string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // ── Lookup via StudentLogin entity (NationalId, V2) ──────────────
            var login = await _db.StudentLogins
                    .Include(sl => sl.Student)
                    .FirstOrDefaultAsync(sl =>
                        sl.NationalId == model.NationalID &&
                        sl.IsActive == true &&
                        sl.IsDeleted == false);

            // ── Fallback: lookup by Email (V1) ───────────────────────────────
            //if (login == null && !string.IsNullOrEmpty(model.))
            //{
            //    login = await _db.StudentLogins
            //        .Include(sl => sl.Student)
            //        .FirstOrDefaultAsync(sl =>
            //            sl.Student.Email!.ToLower() == model.Email.ToLower() &&
            //            sl.IsActive == true &&
            //            sl.IsDeleted == false);
            //}

            if (login == null || !VerifyPassword(model.Password, login.PasswordHash!))
            {
                Console.WriteLine("hello");
                ModelState.AddModelError(string.Empty, "Invalid credentials. Please try again.");
                return View(model);
            }

            var student = login.Student;

            // ── Status gate (both V1 & V2) ───────────────────────────────────
            if (student.Status == StudentStatus.Pending)
            {
                ModelState.AddModelError(string.Empty,
                    "Your registration is waiting for admin approval.");
                return View(model);
            }

            if (student.Status == StudentStatus.Rejected)
            {
                ModelState.AddModelError(string.Empty,
                    "Your registration was rejected. Please contact the dormitory office.");
                return View(model);
            }

            // ── Issue cookie (V1 approach) ───────────────────────────────────
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, student.StudentId.ToString()),
                new Claim(ClaimTypes.Name,           student.FullNameArabic ?? student.Name ?? ""),
                new Claim(ClaimTypes.Email,          student.Email ?? ""),
                new Claim("NationalId",              student.NationalId ?? ""),
                new Claim(ClaimTypes.Role,           "Student")
            };

            await SignInWithCookieAsync(claims, model.RememberMe);

            // ── Update last login (V2) ───────────────────────────────────────
            login.LastLoginAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Dashboard", "Student");
        }

        // ====================================================================
        // ADMIN LOGIN  (V2 — promoted to cookie auth)
        // ====================================================================

        [HttpGet]
        public IActionResult AdminLogin() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var admin = await _db.Admins
                .FirstOrDefaultAsync(a =>
                    a.Username == model.Username &&
                    a.IsActive == true &&
                    a.IsDeleted == false);

            if (admin == null || !VerifyPassword(model.Password, admin.PasswordHash!))
            {
                ModelState.AddModelError(string.Empty,
                    "Invalid username or password.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, admin.AdminId.ToString()),
                new Claim(ClaimTypes.Name,           admin.Name ?? ""),
                new Claim("AdminRole",               admin.Role ?? ""),
                new Claim(ClaimTypes.Role,           "Admin")
            };

            await SignInWithCookieAsync(claims, isPersistent: false);

            return RedirectToAction("Dashboard", "Adminpanel");
        }

        // ====================================================================
        // LOGOUT  (V1 cookie sign-out, clears everything)
        // ====================================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(StudentLogin));
        }

        // ====================================================================
        // ACCESS DENIED  (V1)
        // ====================================================================

        public IActionResult AccessDenied() => View();

        // ====================================================================
        // PRIVATE HELPERS
        // ====================================================================

        /// <summary>
        /// SHA-256 hash stored as a byte array (V2 approach).
        /// </summary>
        private static byte[] HashPassword(string password)
        {
            using var sha = SHA256.Create();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        /// <summary>
        /// Constant-time byte comparison to prevent timing attacks.
        /// </summary>
        private static bool VerifyPassword(string password, byte[] storedHash)
        {
            var incoming = HashPassword(password);
            return incoming.SequenceEqual(storedHash);
        }

        /// <summary>
        /// Issues a cookie for the supplied claims.
        /// Session cookie when isPersistent = false, 14-day cookie otherwise.
        /// </summary>
        private async Task SignInWithCookieAsync(IEnumerable<Claim> claims,
                                                  bool isPersistent = false)
        {
            var identity = new ClaimsIdentity(claims,
                                CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var props = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                ExpiresUtc = isPersistent
                    ? DateTimeOffset.UtcNow.AddDays(14)
                    : DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                props);
        }
    }
}