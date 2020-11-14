using AutoMapper;
using GymHub.Automapper.AutomapperProfiles;
using GymHub.Common;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.DTOs;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private IProductCommentService productCommentService;
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

            //Set up productService
            this.productService = new ProductService(context, this.mapper);

            //Set up productCommentService
            this.productCommentService = new ProductCommentService(context);
        }

        public async Task SeedAsync()
        {
            //Seeder methods
            await SeedGendersAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedProductsAsync();
            await SeedProductsCommentsAsync();

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
                if (await this.userService.UserIsTakenAsync(newUser.Username, newUser.Password, newUser.Email) == false)
                {
                    await this.userService.CreateUserAsync(newUser);
                }
            }

            return true;
        }

        private async Task<bool> SeedProductsAsync()
        {
            var productsDTOs = JsonSerializer.Deserialize<List<ProductDTO>>(File.ReadAllText($"../GymHub.Services/SeederFolder/SeedJSON/Products.json"));
            var products = productsDTOs
                .Select(x => new Product 
                { 
                    Name = x.Name,
                    Model = x.Model,
                    Warranty = x.Warranty,
                    QuantityInStock = x.QuantityInStock,
                    MainImage = x.MainImage,
                    Description = x.Description,
                    Price = x.Price,
                    ProductRatings = x.ProductRatings
                        .Select((pr) => new ProductRating 
                        {
                            Rating = pr.Rating,
                            UserId = this.userService.GetUserId(pr.Username)
                        }).ToList()
                }).ToList();

            foreach (var product in products)
            {
                if(await this.productService.ProductExistsByModelAsync(product.Model) == false)
                {
                    await this.productService.AddAsync(product);
                }
            }
            return true;
        }

        private async Task<bool> SeedProductsCommentsAsync()
        {
            var productsCommentsDTOs = JsonSerializer.Deserialize<List<ProductCommentDTO>>(File.ReadAllText($"../GymHub.Services/SeederFolder/SeedJSON/ProductsComments.json"));
            var productsComments = productsCommentsDTOs
                .Select(x => new ProductComment
                {
                    CommentedOn = x.CommentedOn,
                    ParentCommentId = x.ParentCommentId,
                    UserId = this.userService.GetUserId(x.Username),
                    ProductId = this.productService.GetProductId(x.ProductModel),
                    Id = x.Id,
                    Text = x.Text
                }).ToList();

            foreach (var productComment in productsComments)
            {
                if (await this.productCommentService.CommentExists(productComment) == false)
                {
                    await this.productCommentService.AddAsync(productComment);
                }
            }
            return true;
        }
    }
}
