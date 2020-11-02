using GymHub.Common;
using GymHub.Data.Models;
using GymHub.Web.Data;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
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
        public Seeder(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext();
            this.genderService = new GenderService(context);

            //Set up roleService
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            this.roleService = new RoleService(context, roleManager);

            //Set up userService
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            this.userService = new UserService(context, this.roleService as RoleService, this.genderService as GenderService, userManager);
            this.productService = new ProductService(context);
        }

        //Currently unused
        public void Seed(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            //Seeder methods
            SeedGenders();
            SeedRoles();
            //SeedUsers();
            //SeedProducts();
        }

        public async Task SeedAsync()
        {
            //Seeder methods
            await SeedGenders();
            await SeedRoles();
            //SeedUsers();
            //SeedProducts();
        }

        private async Task<bool> SeedRoles()
        {
            //Seed data
            var roles = new List<string>();
            roles.Add(GlobalConstants.AdminRoleName);
            roles.Add(GlobalConstants.NormalUserRoleName);

            try
            {
                foreach (var roleName in roles)
                {
                    if (await this.roleService.RoleExistsAsync(roleName) == false)
                    {
                        await this.roleService.AddAsync(roleName);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private async Task<bool> SeedGenders()
        {
            //Seed data
            var genders = new List<string>();
            genders.Add(GlobalConstants.MaleGenderName);
            genders.Add(GlobalConstants.FemaleGenderName);
            genders.Sort();

            try
            {
                foreach (var genderName in genders)
                {
                    if (await this.genderService.GenderExistsAsync(genderName) == false)
                    {
                        await this.genderService.AddAsync(genderName);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private async Task<bool> SeedUsers()
        {
            try
            {
                await this.userService.RegisterNormalUserAsync(new RegisterUserInputModel
                {
                    FirstName = "Rosen",
                    MiddleName = "Andreev",
                    LastName = "Kolev",
                    Username = "rosenkolev1",
                    Password = "rosenkolev1",
                    DateOfBirth = new DateTime(2002, 9, 17),
                    Email = "rosenandreevkolev@abv.bg",
                    GenderId = await this.genderService.GetGenderIdByNameAsync("Male")
                });
            }
            catch
            {
                return false;
            }
            return true;
        }

        private async Task<bool> SeedProducts()
        {
            try
            {
                var products = JsonSerializer.Deserialize<List<AddProductInputModel>>(File.ReadAllText("SeederFolder/SeedJSON/Products.json"));
                foreach (var product in products)
                {
                    await this.productService.AddAsync(product);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
