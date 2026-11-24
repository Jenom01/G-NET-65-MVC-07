using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem_BLL.ViewModels.Plans
{
    public class PlanEditViewModel
    {
        public string? PlanName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 200 characters")]
        public string Description { get; set; } = default!;

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days")]
        [Display(Name = "Duration (days)")]
        public int? DurationDays { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 100000, ErrorMessage = "Price must be between 0 and 100,000")]
        [Display(Name = "Price (EGP)")]
        public decimal? Price { get; set; }
    }
}
