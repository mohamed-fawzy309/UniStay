using System.ComponentModel.DataAnnotations;

namespace DormitorySystem.Models.ViewModels
{
    // ─── Registration ────────────────────────────────────────────────────────────
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Student ID is required.")]
        [StringLength(20)]
        [Display(Name = "Student ID Number")]
        public string StudentNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course is required.")]
        [StringLength(100)]
        [Display(Name = "Course / Program")]
        public string Course { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Enter a valid phone number.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(300)]
        [Display(Name = "Home Address")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    // ─── Login ───────────────────────────────────────────────────────────────────
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }

    // ─── Admin Approval ──────────────────────────────────────────────────────────
    public class ApprovalActionViewModel
    {
        public int StudentId { get; set; }

        [StringLength(500)]
        [Display(Name = "Remarks (optional)")]
        public string? Remarks { get; set; }
    }
}