// ViewModels/ApplyViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace UniStay.ViewModels;

public class ApplyViewModel
{
    [Required(ErrorMessage = "يرجى اختيار المبنى")]
    public int BuildingId { get; set; }

    [Required(ErrorMessage = "يرجى اختيار الغرفة")]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "يرجى إدخال السنة الدراسية")]
    public string AcademicYear { get; set; } = string.Empty;

    public string? HousingPreference { get; set; }  // e.g. "هادئ" / "اجتماعي"
    public string? Notes { get; set; }
}