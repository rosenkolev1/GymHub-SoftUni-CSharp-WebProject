using GymHub.Data;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;
        private readonly IRoleService roleService;
        private readonly IGenderService genderService;
        private readonly UserManager<User> userManager;
        public UserService(ApplicationDbContext context, IRoleService roleService, IGenderService genderService, UserManager<User> userManager)
        {
            this.context = context;
            this.roleService = roleService;
            this.genderService = genderService;
            this.userManager = userManager;
        }

        public async Task<User> CreateNormalUserAsync(RegisterUserInputModel inputModel)
        {
            var newUser = new User()
            {
                FirstName = inputModel.FirstName,
                MiddleName = inputModel.MiddleName,
                LastName = inputModel.LastName,
                DateOfBirth = inputModel.DateOfBirth,
                UserName = inputModel.Username,
                Email = inputModel.Email,
                DeletedOn = null,
                IsDeleted = false,
                RegisteredOn = DateTime.UtcNow,
                RoleId = await this.roleService.GetNormalUserRoleIdAsync(),
                GenderId = inputModel.GenderId,
            };
            var newUserPassword = inputModel.Password;

            await this.userManager.CreateAsync(newUser, newUserPassword);

            await this.userManager.AddToRoleAsync(newUser, (await this.roleService.GetNormalUserRoleAsync()).Name);

            await this.context.SaveChangesAsync();

            return newUser;
        }

        public async Task<string> GetIdByUsernameAndPasswordAsync(LoginUserInputModel inputModel)
        {
            if (await UserExistsAsync(inputModel.Username, inputModel.Password) == false)
            {
                return null;
            }
            return (await this.userManager.FindByNameAsync(inputModel.Username)).Id;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return this.context.Users.Any(x => x.UserName == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return this.context.Users.Any(x => x.Email == email);
        }

        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            return this.context.Users.Any(x => x.PhoneNumber == phoneNumber);
        }

        public async Task<bool> UserExistsAsync(string username, string password)
        {
            var user = await this.userManager.FindByNameAsync(username);
            if (user == null)
            {
                return false;
            }
            var passwordIsCorrect = await this.userManager.CheckPasswordAsync(user, password);

            return passwordIsCorrect;
        }

        public async Task<User> GetUserAsync(LoginUserInputModel inputModel)
        {
            if (await UserExistsAsync(inputModel.Username, inputModel.Password) == false)
            {
                return null;
            }
            return await this.userManager.FindByNameAsync(inputModel.Username);
        }
    }
}
