using GymHub.Common;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Web.Models.InputModels
{
    public class LoginUserInputModel
    {
        [MaxLength(GlobalConstants.UsernameLengthMax)]
        [MinLength(GlobalConstants.UsernameLengthMin)]
        [Required]
        public string Username { get; set; }
        [Required]
        [MaxLength(GlobalConstants.PasswordLengthMax)]
        [MinLength(GlobalConstants.PasswordLengthMin)]
        public string Password { get; set; }
    }
}
