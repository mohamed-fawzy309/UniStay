using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniStay.Data;
using UniStay.Models;

namespace UniStay.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DormitoryDbContext _db;

        public HomeController(ILogger<HomeController> logger, DormitoryDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index() => View();
        public IActionResult Dates() => View();
        public IActionResult Accept() => View();

        // ?????????????????????????????????????????????????????????????
        //  GET /Home/CheckAcceptance?id=29901011234567
        //  Called by the Accept page via fetch()
        // ?????????????????????????????????????????????????????????????
        [HttpGet]
        public async Task<IActionResult> CheckAcceptance(string id)
        {
            // Basic validation — must be 14 digits
            if (string.IsNullOrWhiteSpace(id) || !Regex.IsMatch(id, @"^\d{14}$"))
                return BadRequest(new { status = "invalid" });

            var student = await _db.Students
                .AsNoTracking()
                .Where(s => s.NationalId == id && s.IsDeleted != true)
                .Select(s => new
                {
                    s.Status,
                    Name = s.FullNameArabic ?? s.Name,
                    s.Faculty,
                    s.Grade,
                    s.StudyType,
                    s.HousingType,
                    s.StudentCode
                })
                .FirstOrDefaultAsync();

            if (student is null)
                return Ok(new { status = "not_found" });

            return student.Status switch
            {
                StudentStatus.Approved => Ok(new
                {
                    status = "accepted",
                    name = student.Name,
                    faculty = student.Faculty,
                    grade = student.Grade,
                    studyType = student.StudyType,
                    seat = student.StudentCode
                }),

                StudentStatus.Pending => Ok(new
                {
                    status = "under_review",
                    name = student.Name,
                    faculty = student.Faculty,
                    grade = student.Grade,
                    studyType = student.StudyType
                }),

                _ => Ok(new { status = "not_found" })   // Rejected or any future status
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}