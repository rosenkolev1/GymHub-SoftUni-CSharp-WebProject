using GymHub.Common;
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
    }
}
