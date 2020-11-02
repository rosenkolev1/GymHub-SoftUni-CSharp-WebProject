using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using GymHub.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        public async Task<IActionResult> Register()
        {
            if(this.User.Identity.IsAuthenticated == true)
            {
                return this.Redirect("/");
            }

            var viewModel = new RegisterUserViewModel
            {
                Genders = await this.genderService.GetAllGendersAsync()
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserInputModel inputModel)
        {
            if (this.User.Identity.IsAuthenticated == true)
            {
                return this.Redirect("/");
            }

            await this.userService.RegisterNormalUserAsync(inputModel);
            return this.Redirect("/Users/Login");
        }
    }
}
