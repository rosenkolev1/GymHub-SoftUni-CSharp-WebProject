using GymHub.Common;
using GymHub.Data.Data;
using GymHub.Web.Models.CustomAttributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GymHub.Web.Models.InputModels
{
    public class RegisterUserInputModel : IValidatableObject
    {
        [MaxLength(GlobalConstants.FirstNameLengthMax, ErrorMessage = "First Name must be at most 30 characters long.")]
        [MinLength(GlobalConstants.FirstNameLengthMin, ErrorMessage = "First Name must be at least 2 characters long.")]
        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(GlobalConstants.FirstNameLengthMax, ErrorMessage = "First Name must be at most 30 characters long.")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [MaxLength(GlobalConstants.LastNameLengthMax, ErrorMessage = "Last Name must be at most 30 characters long.")]
        [MinLength(GlobalConstants.LastNameLengthMin, ErrorMessage = "Last Name must be at least 2 characters long.")]
        [Required(ErrorMessage = "Last Nameis required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(GlobalConstants.EmailLengthMax, ErrorMessage = "Email must be at most 60 characters long.")]
        [MinLength(GlobalConstants.EmailLengthMin, ErrorMessage = "Email must be at least 4 characters long.")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [Display(Name = nameof(Email))]
        public string Email { get; set; }

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

        [Compare(nameof(Password), ErrorMessage = "Passwords should match")]
        [Display(Name = "Repeat password")]
        [Required(ErrorMessage = "Confirm password is required.")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Date of birth must be a valid date")]
        [RangeOfDate]
        [Required(ErrorMessage = "Date of birth is required.")]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender is required.")]
        public string GenderId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Get required services
            var dbContext = validationContext.GetRequiredService<ApplicationDbContext>();
            var validationResults = new List<ValidationResult>();

            //Validate gender exists in the database
            if (dbContext.Genders.Any(x => x.Id == GenderId) == false)
            {
                validationResults.Add(new ValidationResult($"Gender should be a valid gender from: {string.Join(", ", dbContext.Genders.Select(x => x.Name).ToList())}", new List<string> { nameof(GenderId) }));
            }

            return validationResults;
        }
    }
}
