using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using GymHub.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymHub.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly IGenderService genderService;
        private readonly SignInManager<User> signInManager;
        public UsersController(IUserService userService, IGenderService genderService, SignInManager<User> signInManager)
        {
            this.userService = userService;
            this.genderService = genderService;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> Register()
        {
            if (this.User.Identity.IsAuthenticated == true)
            {
                return this.Redirect("/error");
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
                return this.Redirect("/error");
            }

            await this.userService.CreateNormalUserAsync(inputModel);
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
        public async Task<IActionResult> Login(LoginUserInputModel inputModel)
        {
            if (this.User.Identity.IsAuthenticated == true)
            {
                return this.Redirect("/error");
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

        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return this.Redirect("/");
        }
    }
}
