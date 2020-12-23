using AutoMapper;
using GymHub.Common;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.DTOs;
using GymHub.Services.Common;
using GymHub.Services.ServicesFolder.GenderService;
using GymHub.Services.ServicesFolder.RoleService;
using GymHub.Web.Models.InputModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Services
{
    public class UserService : DeleteableEntityService, IUserService
    {
        private readonly IRoleService roleService;
        private readonly IGenderService genderService;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public UserService(ApplicationDbContext context, IRoleService roleService, IGenderService genderService, UserManager<User> userManager, IMapper mapper)
            :base(context)
        {
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

        public bool UsernameExists(string username)
        {
            return this.context.Users.Any(x => x.UserName == username);
        }

        public bool EmailExists(string email)
        {
            return this.context.Users.Any(x => x.Email == email);
        }

        public bool PhoneNumberExists(string phoneNumber)
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
            if (roles.Length == 0)
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
            newUser.GenderId = this.genderService.GetGenderIdByName(userDTO.GenderName);

            var newUserPassword = userDTO.Password;

            await this.userManager.CreateAsync(newUser, newUserPassword);

            foreach (var roleName in roleNames)
            {
                var role = this.roleService.GetRole(roleName);
                await this.userManager.AddToRoleAsync(newUser, role.Name);
            }

            await this.context.SaveChangesAsync();

            return newUser;
        }

        public bool UserIsTaken(string username, string password, string email, string phoneNumber = null, bool hardCheck = false)
        {
            var passwordHash = this.userManager.PasswordHasher.HashPassword(null, password);
            return this.context.Users.IgnoreAllQueryFilters(hardCheck).Where(x => x.PhoneNumber != null).Any(x => x.UserName == username || x.PasswordHash == password || x.Email == email || x.PhoneNumber == phoneNumber);
        }

        public string GetUserId(string username, bool hardCheck = false)
        {
            return this.context.Users.IgnoreAllQueryFilters(hardCheck).FirstOrDefault(x => x.UserName == username).Id;
        }

        public string GetEmail(string userId)
        {
            return this.context.Users.FirstOrDefault(x => x.Id == userId).Email;
        }

        public User GetUser(string userId)
        {
            return this.context.Users.FirstOrDefault(x => x.Id == userId);
        }

        public User GetUserByUsername(string username)
        {
            return this.context.Users
                .FirstOrDefault(x => x.UserName == username);
        }

        public User GetAdminUser()
        {
            //TODO edit the database a bit with the whole roles thing
            var adminRole = this.roleService.GetRole(GlobalConstants.AdminRoleName);
            var adminRoleId = adminRole.Id;

            return this.context.Users
                .Where(x => x.Roles.Select(y => y.RoleId).Contains(adminRoleId))
                .First();
        }
    }
}
