using GymHub.Data.Models;
using GymHub.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    }
}
