using AutoMapper;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.DTOs;
using GymHub.Web.Models.InputModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;
        private readonly IRoleService roleService;
        private readonly IGenderService genderService;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        public UserService(ApplicationDbContext context, IRoleService roleService, IGenderService genderService, UserManager<User> userManager, IMapper mapper)
        {
            this.context = context;
            this.roleService = roleService;
            this.genderService = genderService;
            this.userManager = userManager;
            this.mapper = mapper;
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

        public async Task<User> CreateUserAsync(RegisterUserInputModel inputModel, params Role[] roles)
        {
            if(roles.Length == 0)
            {
                throw new ArgumentException("No roles have been give to this user");
            }

            var newUser = mapper.Map<User>(inputModel);

            var newUserPassword = inputModel.Password;

            await this.userManager.CreateAsync(newUser, newUserPassword);

            foreach (var role in roles)
            {
                await this.userManager.AddToRoleAsync(newUser, role.Name);
            }

            await this.context.SaveChangesAsync();

            return newUser;
        }

        public async Task<User> CreateUserAsync(UserDTO userDTO)
        {
            var newUser = mapper.Map<User>(userDTO);
            var roleNames = userDTO.RoleNames;
            newUser.GenderId = await this.genderService.GetGenderIdByNameAsync(userDTO.GenderName);

            var newUserPassword = userDTO.Password;

            await this.userManager.CreateAsync(newUser, newUserPassword);

            foreach (var roleName in roleNames)
            {
                var role = await this.roleService.GetRoleAsync(roleName);
                await this.userManager.AddToRoleAsync(newUser, role.Name);
            }

            await this.context.SaveChangesAsync();

            return newUser;
        }

        public async Task<bool> UserIsTakenAsync(string username, string password, string email, string phoneNumber = null)
        {
            var passwordHash = this.userManager.PasswordHasher.HashPassword(null, password);
            return this.context.Users.Where(x => x.PhoneNumber != null).Any(x => x.UserName == username || x.PasswordHash == password || x.Email == email || x.PhoneNumber == phoneNumber);
        }
    }
}
