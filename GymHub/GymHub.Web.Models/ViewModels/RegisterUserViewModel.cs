using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Web.Models.ViewModels
{
    public class RegisterUserViewModel
    {
        public List<Gender> Genders { get; set; }
        public RegisterUserInputModel RegisterUserInputModel { get; set; }
    }
}
