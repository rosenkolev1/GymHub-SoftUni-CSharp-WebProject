﻿using GymHub.Common;
using GymHub.Data.Models;
using GymHub.Web.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using GymHub.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace GymHub.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IGenderService genderService;
        private readonly SignInManager<User> signInManager;
        public UsersController(IUserService userService, IGenderService genderService, IRoleService roleService, SignInManager<User> signInManager)
        {
            this.userService = userService;
            this.genderService = genderService;
            this.signInManager = signInManager;
            this.roleService = roleService;
        }

        public async Task<IActionResult> Register()
        {
            if (this.User.Identity.IsAuthenticated == true)
            {
                return this.Redirect("/Home/Error");
            }

            var viewModel = new ComplexModel<RegisterUserInputModel, RegisterUserViewModel>
            {
                ViewModel= new RegisterUserViewModel { Genders = await this.genderService.GetAllGendersAsync() }
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(ComplexModel<RegisterUserInputModel, RegisterUserViewModel> complexModel)
        {
            if (this.User.Identity.IsAuthenticated == true)
            {
                return this.Redirect("/Home/Error");
            }

            if(this.ModelState.IsValid == false)
            {
                return this.Redirect("/SomethingInvalid");
            }

            var inputModel = complexModel.InputModel;

            if (await this.userService.UserIsTakenAsync(inputModel.Username, inputModel.Password, inputModel.Email) == true)
            {
                return this.Redirect("/SomethingInvalid");
            }

            await this.userService.CreateUserAsync(inputModel, await this.roleService.GetRoleAsync(GlobalConstants.NormalUserRoleName));
            return this.Redirect("/Users/Login");
        }

        public async Task<IActionResult> Login()
        {
            if (this.User.Identity.IsAuthenticated == true)
            {
                return this.Redirect("/error");
            }

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserInputModel inputModel)
        {
            if (this.User.Identity.IsAuthenticated == true)
            {
                return this.Redirect("/Home/Error");
            }

            if(this.ModelState.IsValid == false)
            {
                return this.Redirect("/SomethingInvalid");
            }

            var username = inputModel.Username;
            var password = inputModel.Password;

            if (await this.userService.UserExistsAsync(username, password) == false)
            {
                return this.Redirect("/error");
            }

            var newUser = await this.userService.GetUserAsync(inputModel);
            await this.signInManager.SignInAsync(newUser, false);

            return this.Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return this.Redirect("/");
        }
    }
}
