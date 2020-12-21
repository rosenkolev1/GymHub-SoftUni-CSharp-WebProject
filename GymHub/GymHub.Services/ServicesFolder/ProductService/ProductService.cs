using AutoMapper;
using GymHub.Common;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using GymHub.Services.ServicesFolder.CategoryService;
using GymHub.Services.ServicesFolder.ProductImageService;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using GymHub.Web.Models.ViewModels.Products.All;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.ProductService
{
    public class ProductService : DeleteableEntityService, IProductService
    {
        private readonly IMapper mapper;
        private readonly IProductImageService productImageService;
        private readonly ICategoryService categoryService;

        public ProductService(ApplicationDbContext context, IMapper mapper, IProductImageService productImageService, ICategoryService categoryService)
            : base(context)
        {
            this.mapper = mapper;
            this.productImageService = productImageService;
            this.categoryService = categoryService;
        }      

        public bool ProductExistsById(string id)
        {
            return context.Products.Any(x => x.Id == id);
        }

        public bool ProductExistsByName(string name)
        {
            return context.Products.Any(x => x.Name == name);
        }
        public bool ProductExistsByModel(string model, bool hardCheck = false)
        {
            return context.Products.IgnoreAllQueryFilters(hardCheck).Any(x => x.Model == model);
        }      

        public Product GetProductById(string productId, bool withNavigationalProperties)
        {
            if (withNavigationalProperties)
            {
                return context.Products
                .Include(x => x.ProductComments)
                .ThenInclude(pc => pc.User)
                .Include(x => x.ProductRatings)
                .ThenInclude(x => x.User)
                .Include(x => x.Images)
                .FirstOrDefault(x => x.Id == productId);
            }
            else
            {
                return context.Products.FirstOrDefault(x => x.Id == productId);
            }
        }

        public async Task AddAsync(Product product)
        {
            await context.AddAsync(product);
            await context.SaveChangesAsync();
        }

        public double GetAverageRating(List<ProductRating> productRatings)
        {
            var ratingsCount = productRatings.Count;
            var ratingsSum = productRatings.Sum(x => x.Rating);
            var ratingIncrement = 0.5d;

            if (ratingsSum == 0 || ratingsCount == 0) return 0;

            var ratingAverage = RoundRatingNumber(ratingsSum / ratingsCount, ratingIncrement);

            return ratingAverage;
        }

        private double RoundRatingNumber(double number, double increment)
        {
            if (number % increment == 0) return number;
            var incrementedNumber = 0d;
            while (true)
            {
                if (number - incrementedNumber <= increment)
                {
                    var distanceToFloor = number - incrementedNumber;
                    var distanceToCeiling = Math.Abs(increment - distanceToFloor);
                    var neededDistance = distanceToFloor >= distanceToCeiling ? distanceToCeiling : -distanceToFloor;
                    return number + neededDistance;
                }
                incrementedNumber += increment;
            }
        }

        public string GetShordDescription(string description, int stringLength)
        {
            var returnString = new StringBuilder();
            var descriptionWords = description.Split(" ").ToList();
            foreach (var word in descriptionWords)
            {
                returnString.Append(word);
                if (returnString.ToString().Length >= stringLength) break;
                returnString.Append(" ");
            }
            if (returnString.ToString().EndsWith('.'))
            {
                returnString.Append("..");
            }
            else
            {
                returnString.Append("...");
            }
            return returnString.ToString();
        }

        public string GetProductId(string model, bool hardCheck = false)
        {
            return context.Products.IgnoreAllQueryFilters(hardCheck).FirstOrDefault(x => x.Model == model).Id;
        }

        public async Task AddRatingAsync(string commentId, string productId, string userId, double rating)
        {
            var newRating = new ProductRating
            {
                ProductCommentId = commentId,
                ProductId = productId,
                UserId = userId,
                Rating = rating
            };
            await context.AddAsync(newRating);
            await context.SaveChangesAsync();
        }

        public bool ProductRatingExists(ProductRating productRating, bool hardCheck = false)
        {
            return context.ProductsRatings.IgnoreAllQueryFilters(hardCheck).Any(x => x.ProductId == productRating.ProductId && x.UserId == productRating.UserId);
        }

        public ProductRating GetProductRating(string userId, string productId)
        {
            return context.ProductsRatings.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId);
        }

        public bool ProductRatingExists(string userId, string productId)
        {
            return context.ProductsRatings.Any(x => x.ProductId == productId && x.UserId == userId);
        }

        public async Task EditProductRating(ProductRating productRating, double rating)
        {
            if (productRating != null) productRating.Rating = rating;
            await context.SaveChangesAsync();
        }

        public async Task AddRatingAsync(ProductRating productRating)
        {
            await context.ProductsRatings.AddAsync(productRating);
            await context.SaveChangesAsync();
        }

        public async Task RemoveProductAsync(string productId)
        {
            var productToDelete = GetProductById(productId, false);
            await context.Entry(productToDelete).Collection(x => x.ProductCarts).LoadAsync();

            context.RemoveRange(productToDelete.ProductCarts);

            await DeleteEntityAsync(productToDelete);
        }

        public async Task EditAsync(EditProductInputModel inputModel)
        {
            var productToEdit = this.context.Products
                .Include(x => x.Images)
                .First(x => x.Id == inputModel.Id);

            //Edit images as links
            if (inputModel.ImagesAsFileUploads == false) 
            {
                //Remove all of the images for the product
                productToEdit.Images.Clear();
                this.context.RemoveRange(this.context.ProductsImages
                    .Where(x => x.ProductId == productToEdit.Id));

                //Edit main image link for product
                var mainImage = new ProductImage { Image = inputModel.MainImage, ProductId = productToEdit.Id, IsMain = true };
                productToEdit.Images.Add(mainImage);

                var newAdditionalImages = inputModel.AdditionalImages.Where(x => x != null).ToList();

                //Edit additional images links for product
                for (int i = 0; i < newAdditionalImages.Count; i++)
                {
                    var newImagePath = newAdditionalImages[i];
                    var newImage = new ProductImage { Image = newImagePath, ProductId = productToEdit.Id, Product = productToEdit };
                    productToEdit.Images.Add(newImage);
                }
            }
            //Edit images as uploads
            else if(inputModel.ImagesAsFileUploads == true)
            {
                //Change main image if set to be modified
                if(inputModel.MainImageUploadInfo.IsBeingModified == true)
                {
                    var mainImageInfo = inputModel.MainImageUploadInfo;

                    await this.productImageService.EditImageAsync(mainImageInfo, productToEdit);
                }

                //Change all additional images if set to be modified
                var additionalImagesInfo = inputModel.AdditionalImagesUploadsInfo;
                foreach (var additionalImageInfo in additionalImagesInfo)
                {
                    if(additionalImageInfo.IsBeingModified == true)
                    {
                        await this.productImageService.EditImageAsync(additionalImageInfo, productToEdit);
                        ////Get the main image from the database
                        //var image = this.productImageService.GetImageById(additionalImageInfo.ModifiedImage.Id);

                        ////Upload main image to azure blob storage
                        //var imageUri = await this.productImageService.UploadImageAsync(additionalImageInfo.ImageUpload, productToEdit);

                        ////Set the main image uri in the database
                        //image.Image = imageUri;
                    }
                }
            }

            //Edit all of the simple properties
            productToEdit.Description = inputModel.Description;
            productToEdit.Warranty = inputModel.Warranty;
            productToEdit.Model = inputModel.Model;
            productToEdit.Name = inputModel.Name;
            productToEdit.Price = inputModel.Price;
            productToEdit.QuantityInStock = inputModel.QuantityInStock;

            await context.SaveChangesAsync();
        }

        public bool ProductExistsByName(string name, string excludedProductId)
        {
            return context.Products
                .Where(x => x.Id != excludedProductId)
                .Any(x => x.Name == name);
        }

        public bool ProductExistsByModel(string model, string excludedProductId, bool hardCheck = false)
        {
            return context.Products
                .IgnoreAllQueryFilters(hardCheck)
                .Where(x => x.Id != excludedProductId)
                .Any(x => x.Model == model);
        }

        public string GetProductName(string productId)
        {
            return this.context.Products.First(x => x.Id == productId).Name;
        }

        public string GetProductModel(string productId)
        {
            return this.context.Products.First(x => x.Id == productId).Model;
        }      

        public  IQueryable<Product> FilterProducts(List<ProductFilterOptionsViewModel> productFilterOptions)
        {
            if (productFilterOptions == null || productFilterOptions.Count == 0)
            {
                return this.context.Products.AsQueryable<Product>();
            }

            IQueryable<Product> filteredProducts = this.context.Products.AsQueryable<Product>();

            var allCategories = this.categoryService.GetAllCategories();

            foreach (var filterOption in productFilterOptions)
            {
                var filterValue = filterOption.FilterValue;
                var filterOptionKey = filterOption.FilterName;

                if (filterOptionKey.StartsWith(GlobalConstants.IncludeCategorySplitter))
                { 
                    var categoryFilterName = filterOptionKey.Split(GlobalConstants.IncludeCategorySplitter, 2)[1];
                    var category = allCategories.First(x => x.Name == categoryFilterName);

                    if(filterValue == true)
                    {
                        filteredProducts = filteredProducts
                        .Where(x => x.ProductCategories.Any(y => y.CategoryId == category.Id) == false);
                    }
                }
            }

            filteredProducts = this.context.Products.Except(filteredProducts);
            return filteredProducts;
        }

        public List<ProductViewModel> GetProductsForPage(IQueryable<Product> products, int page)
        {
            var listOfViewModelsForCurrentPage = products
                    .Skip((page - 1) * GlobalConstants.ProductsPerPage)
                    .Take(GlobalConstants.ProductsPerPage)
                    .Select(product => new
                    {
                        product.Id,
                        product.Name,
                        product.Description,
                        product.QuantityInStock,
                        product.Images.First(x => x.IsMain == true).Image,
                        product.Model,
                        product.Price,
                        product.Warranty,
                        product.ProductRatings,
                        ProductSalesCount = product.ProductSales.Count
                    }).ToList();

            var listOfViewModels = listOfViewModelsForCurrentPage
                .Select(product => new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    QuantityInStock = product.QuantityInStock,
                    MainImage = product.Image,
                    Model = product.Model,
                    Price = product.Price,
                    Warranty = product.Warranty,
                    ProductSalesCount = product.ProductSalesCount,
                    ShortDescription = this.GetShordDescription(product.Description, 40),
                    ProductRatingViewModel = new ProductRatingViewModel(this.GetAverageRating(product.ProductRatings.ToList()))
                }).ToList();

            return listOfViewModels;
        }

        public List<ProductViewModel> GetProductsFrom(int page, List<ProductFilterOptionsViewModel> productFilterOptions)
        {


            var filteredProducts = this.FilterProducts(productFilterOptions);

            return this.GetProductsForPage(filteredProducts, page);
       
        }
    }
}
