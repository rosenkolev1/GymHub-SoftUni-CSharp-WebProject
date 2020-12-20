using GymHub.Common;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Web.Models.InputModels
{
    public class LoginUserInputModel
    {
        public LoginUserInputModel()
        {
            RememberMe = true;
        }

        [MaxLength(GlobalConstants.UsernameLengthMax, ErrorMessage = "Username must be at most 30 characters long.")]
        [MinLength(GlobalConstants.UsernameLengthMin, ErrorMessage = "Username must be at least 6 characters long.")]
        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = nameof(Username))]
        public string Username { get; set; }

        [MaxLength(GlobalConstants.PasswordLengthMax, ErrorMessage = "Password must be at most 30 characters long.")]
        [MinLength(GlobalConstants.PasswordLengthMin, ErrorMessage = " Password must be at least 8 characters long.")]
        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = nameof(Password))]
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}
