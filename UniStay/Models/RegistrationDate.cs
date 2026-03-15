// Models/RegistrationDate.cs
namespace UniStay.Models
{
    public class RegistrationDate
    {
        public int Id { get; set; }
        public string Category { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}