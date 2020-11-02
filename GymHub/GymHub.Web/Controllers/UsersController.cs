using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using GymHub.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

        public IActionResult Register()
        {
            if(this.User.Identity.IsAuthenticated == false)
            {
                return this.Redirect("/");
            }

            var viewModel = new RegisterUserViewModel
            {
                Genders = this.genderService.GetAllGenders()
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Register(RegisterUserInputModel inputModel)
        {
            return this.Redirect("/");
        }
    }
}
