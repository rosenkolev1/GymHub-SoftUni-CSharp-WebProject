using Microsoft.AspNetCore.Mvc;
using System;

namespace GymHub.Web.Models.InputModels
{
    public class RegisterUserInputModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string GenderId { get; set; }
    }
}
