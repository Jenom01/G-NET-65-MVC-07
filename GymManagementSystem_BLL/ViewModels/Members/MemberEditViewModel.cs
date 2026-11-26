using GymManagementSystem_BLL.Validation;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem_BLL.ViewModels.Members
{
    public class MemberEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public bool HasPhoto { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email must be 100 characters or less")]
        [RegularExpression(ValidationPatterns.Email, ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(ValidationPatterns.EgyptianMobile,
            ErrorMessage = "Phone must be a valid Egyptian mobile number (e.g. 01012345678)")]
        public string Phone { get; set; } = default!;

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
    }
}
