using AutoMapper;
using GymHub.Common;
using GymHub.Common.AutomapperProfiles;
using GymHub.Data;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.DTOs;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymHub.Services.SeederFolder
{
    public class Seeder
    {
        private IGenderService genderService;
        private IRoleService roleService;
        private IUserService userService;
        private IProductService productService;
        private IMapper mapper;
        public Seeder(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext();
            this.genderService = new GenderService(context);


            //Set up roleService
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            this.roleService = new RoleService(context, roleManager);

            //Set up automapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(UserProfile));
            });
            this.mapper = mapperConfig.CreateMapper();

            //Set up userService
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            this.userService = new UserService(context, this.roleService as RoleService, this.genderService as GenderService, userManager, this.mapper);
            this.productService = new ProductService(context, this.mapper);
        }

        public async Task SeedAsync()
        {
            //Seeder methods
            await SeedGendersAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedProductsAsync();
        }

        private async Task<bool> SeedRolesAsync()
        {
            //Seed data
            var roles = new List<string>();
            roles.Add(GlobalConstants.AdminRoleName);
            roles.Add(GlobalConstants.NormalUserRoleName);

            foreach (var roleName in roles)
            {
                if (await this.roleService.RoleExistsAsync(roleName) == false)
                {
                    await this.roleService.AddAsync(roleName);
                }
            }

            return true;
        }

        private async Task<bool> SeedGendersAsync()
        {
            //Seed data
            var genders = new List<string>();
            genders.Add(GlobalConstants.MaleGenderName);
            genders.Add(GlobalConstants.FemaleGenderName);
            genders.Sort();

            foreach (var genderName in genders)
            {
                if (await this.genderService.GenderExistsAsync(genderName) == false)
                {
                    await this.genderService.AddAsync(genderName);
                }
            }

            return true;
        }

        private async Task<bool> SeedUsersAsync()
        {
            var users = JsonSerializer.Deserialize<List<UserDTO>>(File.ReadAllText($"../GymHub.Services/SeederFolder/SeedJSON/Users.json"));

            foreach (var newUser in users)
            {
                if (await this.userService.UserExistsAsync(newUser.Username, newUser.Password) == false)
                {
                    await this.userService.CreateUserAsync(newUser);
                }
            }

            return true;
        }

        private async Task<bool> SeedProductsAsync()
        {
            var products = JsonSerializer.Deserialize<List<AddProductInputModel>>(File.ReadAllText($"../GymHub.Services/SeederFolder/SeedJSON/Products.json"));
            foreach (var product in products)
            {
                if(await this.productService.ProductExistsByModelAsync(product.Model) == false)
                {
                    await this.productService.AddAsync(product);
                }
            }
            return true;
        }
    }
}
