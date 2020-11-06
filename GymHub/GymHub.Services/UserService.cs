﻿using AutoMapper;
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
        private readonly IMapper mapper;
        public UserService(ApplicationDbContext context, IRoleService roleService, IGenderService genderService, UserManager<User> userManager, IMapper mapper)
        {
            this.context = context;
            this.roleService = roleService;
            this.genderService = genderService;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<User> CreateNormalUserAsync(RegisterUserInputModel inputModel)
        {
            var newUser = await CreateUserAsync(inputModel, false);

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

        public async Task<User> CreateAdminUserAsync(RegisterUserInputModel inputModel)
        {
            var newUser = await CreateUserAsync(inputModel, true);

            await this.userManager.AddToRoleAsync(newUser, (await this.roleService.GetAdminUserAsync()).Name);

            await this.context.SaveChangesAsync();

            return newUser;
        }

        public async Task<User> CreateUserAsync(RegisterUserInputModel inputModel, Role role)
        {
            var newUser = mapper.Map<User>(inputModel);
            newUser.RoleId = isAdmin == false ? await this.roleService.GetNormalUserRoleIdAsync() : await this.roleService.GetAdminUserRoleIdAsync();

            var newUserPassword = inputModel.Password;

            await this.userManager.CreateAsync(newUser, newUserPassword);

            return newUser;
        }
    }
}
