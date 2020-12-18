using AutoMapper;
using GymHub.Automapper.AutomapperProfiles;
using GymHub.Common;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.DTOs;
using GymHub.Services.ServicesFolder.CategoryService;
using GymHub.Services.ServicesFolder.CountryService;
using GymHub.Services.ServicesFolder.GenderService;
using GymHub.Services.ServicesFolder.PaymentMethodService;
using GymHub.Services.ServicesFolder.ProductCommentService;
using GymHub.Services.ServicesFolder.ProductImageService;
using GymHub.Services.ServicesFolder.ProductService;
using GymHub.Services.ServicesFolder.RoleService;
using GymHub.Services.ServicesFolder.SaleService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IGenderService genderService;
        private readonly IRoleService roleService;
        private readonly IUserService userService;
        private readonly IProductService productService;
        private readonly IMapper mapper;
        private readonly IProductCommentService productCommentService;
        private readonly ApplicationDbContext context;
        private readonly ICategoryService categoryService;
        private readonly ICountryService countryService;
        private readonly IPaymentMethodService paymentMethodService;
        private readonly SaleService saleService;
        private readonly IProductImageService productImageService;

        public Seeder(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            this.context = new ApplicationDbContext(connectionString);
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

            //Set up productImageService
            this.productImageService = new ProductImageService(context);

            //Set up productService
            this.productService = new ProductService(context, this.mapper, this.productImageService);

            //Set up productCommentService
            this.productCommentService = new ProductCommentService(context);

            //Set up categoryService
            this.categoryService = new CategoryService(context);

            //Set up countryService
            this.countryService = new CountryService(context);

            //Set up countryService
            this.paymentMethodService = new PaymentMethodService(context);

            //Set up sales service 
            this.saleService = new SaleService(context);
        }

        public async Task SeedAsync()
        {
            //Seeder methods
            //Seed mostly static stuff
            await SeedGendersAsync();
            await SeedRolesAsync();
            await SeedCountriesAsync();
            await SeedPaymentMethodsAsync();
            await SeedSaleStatusesAsync();

            //Seed the users
            await SeedUsersAsync();

            //Seed categories
            await SeedCategories();

            //Seed anything directly product related
            await SeedProductsAsync();
            await SeedProductsImagesAsync();
            await SeedProductsCommentsAndRatingsAsync();
        }

        private async Task<bool> SeedRolesAsync()
        {
            //Seed data
            var roles = new List<string>();
            roles.Add(GlobalConstants.AdminRoleName);
            roles.Add(GlobalConstants.NormalUserRoleName);

            foreach (var roleName in roles)
            {
                if (this.roleService.RoleExists(roleName, true) == false)
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
                if (this.genderService.GenderExists(genderName, true) == false)
                {
                    await this.genderService.AddAsync(genderName);
                }
            }

            return true;
        }

        private async Task<bool> SeedCountriesAsync()
        {
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.PropertyNameCaseInsensitive = true;

            var countries = JsonSerializer.Deserialize<List<CountryDTO>>(File.ReadAllText($"../GymHub.Services/SeederFolder/SeedJSON/Countries.json"), jsonOptions);

            foreach (var country in countries)
            {
                if (this.countryService.CountryExists(country.Code, true) == false)
                {
                    await this.countryService.AddAsync(country.Name, country.Code);
                }
            }

            return true;
        }

        private async Task<bool> SeedPaymentMethodsAsync()
        {
            var paymentMethods = JsonSerializer.Deserialize<List<string>>(File.ReadAllText($"../GymHub.Services/SeederFolder/SeedJSON/PaymentMethods.json"));

            foreach (var paymentMethod in paymentMethods)
            {
                if (this.paymentMethodService.PaymentMethodExistsByName(paymentMethod) == false)
                {
                    await this.paymentMethodService.AddAsync(paymentMethod);
                }
            }

            return true;
        }

        private async Task<bool> SeedSaleStatusesAsync()
        {
            var saleStatuses = new List<SaleStatus>
            {
                new SaleStatus{Id = GlobalConstants.PendingSaleStatus, Name = GlobalConstants.PendingSaleStatus},
                new SaleStatus{Id = GlobalConstants.ConfirmedSaleStatus, Name = GlobalConstants.ConfirmedSaleStatus},
                new SaleStatus{Id = GlobalConstants.DeclinedSaleStatus, Name = GlobalConstants.DeclinedSaleStatus}
            };

            foreach (var saleStatus in saleStatuses)
            {
                if (this.saleService.SaleStatusExists(saleStatus.Id) == false)
                {
                    await this.saleService.AddSaleStatusAsync(saleStatus);
                }
            }

            return true;
        }

        private async Task<bool> SeedUsersAsync()
        {
            var users = JsonSerializer.Deserialize<List<UserDTO>>(File.ReadAllText($"../GymHub.Services/SeederFolder/SeedJSON/Users.json"));

            foreach (var newUser in users)
            {
                if (this.userService.UserIsTaken(newUser.Username, newUser.Password, newUser.Email, null, false) == false)
                {
                    await this.userService.CreateUserAsync(newUser);
                }
            }

            return true;
        }

        private async Task<bool> SeedCategories()
        {
            var categoriesDTOs = JsonSerializer.Deserialize<List<CategoryDTO>>(File.ReadAllText($"../GymHub.Services/SeederFolder/SeedJSON/Categories.json"));
            var categories = categoriesDTOs
                .Select(x => new Category
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

            foreach (var category in categories)
            {
                if (this.categoryService.CategoryExists(category.Id, true) == false)
                {
                    await this.categoryService.AddAsync(category);
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
                    Images = new List<ProductImage>(),
                    Description = x.Description,
                    Price = x.Price,
                    ProductCategories = x.Categories
                        .Select(categoryId => new ProductCategory
                        {
                            CategoryId = categoryId
                        }).ToList()
                }).ToList();

            foreach (var product in products)
            {
                if (this.productService.ProductExistsByModel(product.Model, true) == false)
                {
                    await this.productService.AddAsync(product);
                }
            }
            return true;
        }

        private async Task<bool> SeedProductsCommentsAndRatingsAsync()
        {
            //Seed the product comments initially
            var productsCommentsDTOs = JsonSerializer.Deserialize<List<ProductCommentDTO>>(File.ReadAllText($"../GymHub.Services/SeederFolder/SeedJSON/ProductsComments.json"));
            var productsComments = productsCommentsDTOs
                .Select(x => new ProductComment
                {
                    CommentedOn = x.CommentedOn,
                    ParentCommentId = x.ParentCommentId,
                    UserId = this.userService.GetUserId(x.Username),
                    ProductId = this.productService.GetProductId(x.ProductModel),
                    Id = x.Id,
                    Text = x.Text,
                }).ToList();

            foreach (var productComment in productsComments)
            {
                if (this.productCommentService.CommentExists(productComment, true) == false)
                {
                    await this.productCommentService.AddAsync(productComment);
                }
            }

            //Seed the product ratings
            var productsRatingsDTOs = JsonSerializer.Deserialize<List<ProductRatingDTO>>(File.ReadAllText($"../GymHub.Services/SeederFolder/SeedJSON/ProductsRatings.json"));
            var productsRatings = productsRatingsDTOs
                .Select(x => new ProductRating
                {
                    Id = x.Id,
                    UserId = this.userService.GetUserId(x.Username, true),
                    ProductId = this.productService.GetProductId(x.ProductModel, true),
                    ProductCommentId = x.CommentId,
                    ProductComment = this.productCommentService.GetProductComment(x.CommentId, true),
                    Rating = x.Rating,
                })
                .ToList();

            foreach (var productRating in productsRatings)
            {
                if (this.productService.ProductRatingExists(productRating, true) == false)
                {
                    await this.productService.AddRatingAsync(productRating);
                }
            }

            //Add reference to product comments for product ratings
            foreach (var productRating in productsRatings)
            {
                if(productRating.ProductComment != null)
                {
                    if (productRating.ProductComment.ProductRatingId == null) productRating.ProductComment.ProductRatingId = productRating.Id;
                }
                await this.context.SaveChangesAsync();
            }

            return true;
        }

        private async Task<bool> SeedProductsImagesAsync()
        {
            var productsImagesDTOs = JsonSerializer.Deserialize<List<ProductImageDTO>>(File.ReadAllText($"../GymHub.Services/SeederFolder/SeedJSON/ProductsImages.json"));
            var productsImages = productsImagesDTOs
                .Select(x => new ProductImage
                {
                    Image = x.Image,
                    ProductId = this.productService.GetProductId(x.ProductModel, true),
                    IsMain = x.IsMain != null & x.IsMain != false ? true : false 
                }).ToList();

            foreach (var productImage in productsImages)
            {
                if (this.productImageService.ProductImageExists(productImage.Image, true) == false)
                {
                    await this.productImageService.AddProductImageAsync(productImage);
                }
            }

            var productWithImages = this.context.ProductsImages
                .Select(x => new
                {
                    ProductImages = x.Product.Images,
                    Image = x
                });

            //Assign images to products
            foreach (var productWithImage in productWithImages)
            {
                productWithImage.ProductImages.Add(productWithImage.Image);
            }

            return true;
        }
    }
}
