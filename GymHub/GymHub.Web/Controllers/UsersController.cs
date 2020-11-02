using GymHub.Data.Models;
using GymHub.Services;
using GymHub.Web.InputModels;
using GymHub.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly IGenderService genderService;
        public UsersController(IUserService userService, IGenderService genderService)
        {
            this.userService = userService;
            this.genderService = genderService;
        }

        public IActionResult Register(RegisterUserViewModel viewModel)
        {
            viewModel.Genders = new List<Gender>();
            viewModel.Genders.Add(new Gender { Name = "Male" });
            viewModel.Genders.Add(new Gender { Name = "Female" });
            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Register(RegisterUserInputModel inputModel)
        {
            return this.Redirect("/");
        }
    }
}
