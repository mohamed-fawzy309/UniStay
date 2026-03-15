using System.ComponentModel.DataAnnotations;

namespace UniStay.ViewModels
{
    public class StudentRegisterViewModel
    {
        // ====================================================================
        // STEP 1 — نوع الطالب والجنسية
        // ====================================================================

        [Required(ErrorMessage = "حدد نوع الطالب")]
        [Display(Name = "نوع الطالب")]
        public string StudentStatus { get; set; } = "New";         // New / Returning

        [Required(ErrorMessage = "حدد الجنسية")]
        [Display(Name = "الجنسية")]
        public string Nationality { get; set; } = "Egyptian";      // Egyptian / Foreign

        [Display(Name = "تفضيل السكن")]
        public string? HousingType { get; set; }                   // Standard / Premium

        // ====================================================================
        // STEP 2 — البيانات الشخصية
        // ====================================================================

        [Required(ErrorMessage = "الاسم مطلوب")]
        [Display(Name = "الاسم الرباعي بالعربية")]
        public string FullNameArabic { get; set; } = null!;

        [Required(ErrorMessage = "الجنس مطلوب")]
        [Display(Name = "الجنس")]
        public string Gender { get; set; } = null!;                // Male / Female

        [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
        [Display(Name = "تاريخ الميلاد")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "محل الميلاد")]
        public string? BirthPlace { get; set; }

        [Display(Name = "الديانة")]
        public string? Religion { get; set; }

        // ── الموقع الجغرافي (Cascading selects → IDs) ────────────────────────
        // Form sends GovernorateId / CenterId / CityId, not plain text strings.

        [Display(Name = "المحافظة")]
        public int? GovernorateId { get; set; }

        [Display(Name = "المركز / القسم")]
        public int? CenterId { get; set; }

        [Display(Name = "المنطقة / القرية")]
        public int? CityId { get; set; }

        [Display(Name = "العنوان التفصيلي")]
        public string? Address { get; set; }

        // ── الأكاديمي ─────────────────────────────────────────────────────────
        // Form field is "AcademicYear" (1-7), ViewModel previously called it "Grade".

        [Required(ErrorMessage = "الكلية مطلوبة")]
        [Display(Name = "الكلية / المعهد")]
        public string Faculty { get; set; } = null!;

        [Required(ErrorMessage = "السنة الدراسية مطلوبة")]
        [Display(Name = "السنة الدراسية")]
        public string Grade { get; set; } = null!;          // "1" … "7"

        // ── الاتصال ───────────────────────────────────────────────────────────
        // Form only has one phone field: "Mobile". "Phone" was removed.

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "بريد إلكتروني غير صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = null!;

        [Display(Name = "رقم الهاتف")]
        public string? Mobile { get; set; }

        // ====================================================================
        // STEP 3 — البيانات الخاصة
        // ====================================================================

        // ── مصري فقط ──────────────────────────────────────────────────────────

        [Display(Name = "الرقم القومي")]
        [MaxLength(14, ErrorMessage = "الرقم القومي 14 رقم")]
        public string? NationalID { get; set; }

        [Display(Name = "اسم الأب")]
        public string? FatherName { get; set; }

        [Display(Name = "الرقم القومي للأب")]
        [MaxLength(14, ErrorMessage = "الرقم القومي 14 رقم")]
        public string? FatherNationalID { get; set; }

        [Display(Name = "هاتف الأب")]
        public string? FatherPhone { get; set; }

        [Display(Name = "وظيفة الأب")]
        public string? FatherJob { get; set; }

        [Display(Name = "الحالة الأسرية")]
        public string? ParentStatus { get; set; }                  // BothAlive / FatherOnly / MotherOnly / Orphan / Divorced

        [Display(Name = "الأسرة بالخارج")]
        public bool FamilyAbroad { get; set; }

        [Display(Name = "ذوي احتياجات خاصة")]
        public bool SpecialNeeds { get; set; }

        // ── مستجد فقط ────────────────────────────────────────────────────────

        [Display(Name = "الشعبة / القسم")]
        public string? HighSchoolDivision { get; set; }

        [Display(Name = "مجموع الثانوية")]
        public double? HighSchoolTotal { get; set; }

        [Display(Name = "النسبة المئوية %")]
        [Range(0, 100, ErrorMessage = "النسبة يجب أن تكون بين 0 و 100")]
        public double? HighSchoolPercentage { get; set; }

        [Display(Name = "ثانوية عامة من الخارج")]
        public bool HighSchoolFromAbroad { get; set; }

        // ── قديم فقط ─────────────────────────────────────────────────────────

        [Display(Name = "تقدير العام الماضي")]
        public string? LastYearGrade { get; set; }

        [Display(Name = "نسبة العام الماضي %")]
        [Range(0, 100, ErrorMessage = "النسبة يجب أن تكون بين 0 و 100")]
        public decimal? LastYearPercentage { get; set; }

        // ── وافد فقط ─────────────────────────────────────────────────────────

        [Display(Name = "الجنسية (البلد)")]
        public string? CountryOfOrigin { get; set; }               // SA / LY / SD … / OTHER

        [Display(Name = "حدد الجنسية")]
        public string? CountryOfOriginOther { get; set; }          // shown when CountryOfOrigin == "OTHER"

        [Display(Name = "رقم جواز السفر")]
        public string? PassportNumber { get; set; }

        [Display(Name = "مكان إصدار الجواز")]
        public string? PassportIssuePlace { get; set; }

        // ====================================================================
        // STEP 4 — بيانات ولي الأمر
        // ====================================================================

        [Display(Name = "اسم ولي الأمر")]
        public string? GuardianName { get; set; }

        [Display(Name = "صلة القرابة")]
        public string? GuardianRelation { get; set; }

        [Display(Name = "الرقم القومي لولي الأمر")]
        [MaxLength(14, ErrorMessage = "الرقم القومي 14 رقم")]
        public string? GuardianNationalID { get; set; }

        [Display(Name = "هاتف ولي الأمر")]
        public string? GuardianPhone { get; set; }

        // ====================================================================
        // STEP 5 — كلمة المرور
        // ====================================================================

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "كلمة المرور 8 أحرف على الأقل")]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        [Display(Name = "تأكيد كلمة المرور")]
        public string ConfirmPassword { get; set; } = null!;
    }
}