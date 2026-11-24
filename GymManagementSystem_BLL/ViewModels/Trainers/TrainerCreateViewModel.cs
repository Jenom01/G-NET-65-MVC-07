using GymManagementSystem_BLL.Validation;
using GymManagementSystem_DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem_BLL.ViewModels.Trainers
{
    public class TrainerCreateViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
        [RegularExpression(ValidationPatterns.Letters, ErrorMessage = "Name can only contain letters and spaces")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email must be 100 characters or less")]
        [RegularExpression(ValidationPatterns.Email, ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(ValidationPatterns.EgyptianMobile,
            ErrorMessage = "Phone must be a valid Egyptian mobile number (e.g. 01012345678)")]
        public string Phone { get; set; } = default!;

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateOnly? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender? Gender { get; set; }

        [Required(ErrorMessage = "Building number is required")]
        [Range(1, 9000, ErrorMessage = "Building number must be between 1 and 9000")]
        [Display(Name = "Building Number")]
        public int? BuildingNumber { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City must be between 2 and 30 characters")]
        [RegularExpression(ValidationPatterns.Letters, ErrorMessage = "City can only contain letters and spaces")]
        public string City { get; set; } = default!;

        [Required(ErrorMessage = "Street is required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 30 characters")]
        [RegularExpression(ValidationPatterns.LettersAndDigits, ErrorMessage = "Street can only contain letters, numbers, and spaces")]
        public string Street { get; set; } = default!;

        [Required(ErrorMessage = "Specialty is required")]
        public Speciality? Speciality { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateOfBirth is { } dob && dob >= DateOnly.FromDateTime(DateTime.Today))
                yield return new ValidationResult("Date of birth must be in the past", [nameof(DateOfBirth)]);
        }
    }
}
