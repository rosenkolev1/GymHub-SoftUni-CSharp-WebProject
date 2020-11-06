using System;
using System.Collections.Generic;

namespace GymHub.DTOs
{
    public class UserDTO
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string GenderName { get; set; }
        public List<string> RoleNames { get; set; }
    }
}
