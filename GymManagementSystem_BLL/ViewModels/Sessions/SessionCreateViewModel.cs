using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem_BLL.ViewModels.Sessions
{
    public class SessionCreateViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")]
        public string Description { get; set; } = default!;

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, 25, ErrorMessage = "Capacity must be between 1 and 25")]
        public int? Capacity { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start Date & Time")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End Date & Time")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Trainer is required")]
        [Display(Name = "Trainer")]
        public int? TrainerId { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate.HasValue && EndDate.HasValue && EndDate <= StartDate)
                yield return new ValidationResult("End time must be after start time",
                    [nameof(EndDate)]);
        }
    }
}
