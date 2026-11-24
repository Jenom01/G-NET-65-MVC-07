using GymManagementSystem_DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem_BLL.ViewModels.HealthRecords
{
    public class HealthRecordCreateViewModel
    {
        [Required(ErrorMessage = "Height is required")]
        [Range(50, 300, ErrorMessage = "Height must be between 50 and 300 cm")]
        public decimal? Height { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        [Range(10, 500, ErrorMessage = "Weight must be between 10 and 500 kg")]
        public decimal? Weight { get; set; }

        [Required(ErrorMessage = "Blood type is required")]
        public BloodType? BloodType { get; set; }

        [StringLength(500, ErrorMessage = "Note must be 500 characters or less")]
        public string? Note { get; set; }
    }
}
