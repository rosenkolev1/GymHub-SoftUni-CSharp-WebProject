using GymHub.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace GymHub.Web.Models.InputModels
{
    public class RegisterUserInputModel
    {
        [MaxLength(GlobalConstants.FirstNameLengthMax)]
        [MinLength(GlobalConstants.FirstNameLengthMin)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(GlobalConstants.FirstNameLengthMax)]
        public string MiddleName { get; set; }

        [MaxLength(GlobalConstants.LastNameLengthMax)]
        [MinLength(GlobalConstants.LastNameLengthMin)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(GlobalConstants.EmailLengthMax)]
        [MinLength(GlobalConstants.EmailLengthMin)]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(GlobalConstants.UsernameLengthMax)]
        [MinLength(GlobalConstants.UsernameLengthMin)]
        [Required]
        public string Username { get; set; }

        [MaxLength(GlobalConstants.PasswordLengthMax)]
        [MinLength(GlobalConstants.PasswordLengthMin)]
        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string GenderId { get; set; }
    }
}
