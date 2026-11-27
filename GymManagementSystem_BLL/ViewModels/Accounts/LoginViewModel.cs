using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem_BLL.ViewModels.Accounts
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [Display(Name = "Remember me for 24 hours")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
