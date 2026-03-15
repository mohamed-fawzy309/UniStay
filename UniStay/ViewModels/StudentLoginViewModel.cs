using System.ComponentModel.DataAnnotations;

namespace UniStay.ViewModels
{
    public class StudentLoginViewModel
    {
        [Required(ErrorMessage = "الرقم القومي مطلوب")]
        [Display(Name = "الرقم القومي")]
        public string NationalID { get; set; } = null!;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }

   
}