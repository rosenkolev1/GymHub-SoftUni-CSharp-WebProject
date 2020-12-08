using AutoMapper;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services
{
    public class ProductService : DeleteableEntityService, IProductService
    {
        private readonly IMapper mapper;

        public ProductService(ApplicationDbContext context, IMapper mapper)
            :base(context)
        {
            this.mapper = mapper;
        }
        public async Task AddAsync(string name, string mainImage, decimal price, string description, int warranty, int quantityInStock)
        {
            this.context.Add(new Product
            {
                Name = name,
                MainImage = mainImage,
                Price = price,
                Description = description,
                Warranty = warranty,
                QuantityInStock = quantityInStock,
                IsDeleted = false,
                DeletedOn = null
            });
            await this.context.SaveChangesAsync();
        }

        public async Task AddAsync(AddProductInputModel inputModel)
        {
            await this.context.AddAsync(mapper.Map<Product>(inputModel));
            await this.context.SaveChangesAsync();
        }

        public bool ProductExistsById(string id)
        {
            return this.context.Products.Any(x => x.Id == id);
        }

        public bool ProductExistsByName(string name)
        {
            return this.context.Products.Any(x => x.Name == name);
        }
        public bool ProductExistsByModel(string model, bool hardCheck = false)
        {
            return this.context.Products.IgnoreAllQueryFilter(hardCheck).Any(x => x.Model == model);
        }

        public List<ProductViewModel> GetAllProducts()
        {
            return this.context.Products.Select(product => mapper.Map<ProductViewModel>(product)).ToList();
        }

        public Product GetProductById(string productId)
        {
            return this.context.Products
                .Include(x => x.ProductComments)
                .ThenInclude(pc => pc.User)
                .Include(x => x.ProductRatings)
                .ThenInclude(x => x.User)
                .Include(x => x.AdditionalImages)
                .FirstOrDefault(x => x.Id == productId);
        }

        public async Task AddAsync(Product product)
        {
            await this.context.AddAsync(product);
            await this.context.SaveChangesAsync();
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
            return this.context.Products.IgnoreAllQueryFilter(hardCheck).FirstOrDefault(x => x.Model == model).Id;
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
            await this.context.AddAsync(newRating);
            await this.context.SaveChangesAsync();
        }

        public bool ProductRatingExists(ProductRating productRating, bool hardCheck = false)
        {
            return this.context.ProductsRatings.IgnoreAllQueryFilter(hardCheck).Any(x => x.ProductId == productRating.ProductId && x.UserId == productRating.UserId);
        }

        public ProductRating GetProductRating(string userId, string productId)
        {
            return this.context.ProductsRatings.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId);
        }

        public bool ProductRatingExists(string userId, string productId)
        {
            return this.context.ProductsRatings.Any(x => x.ProductId == productId && x.UserId == userId);
        }

        public async Task EditProductRating(ProductRating productRating, double rating)
        {
            if (productRating != null) productRating.Rating = rating;
            await this.context.SaveChangesAsync();
        }

        public async Task AddRatingAsync(ProductRating productRating)
        {
            await this.context.ProductsRatings.AddAsync(productRating);
            await this.context.SaveChangesAsync();
        }

        public bool ProductImageExists(string imageUrl, bool hardCheck = false)
        {
            return this.context.Products.IgnoreAllQueryFilter(hardCheck).Any(x => x.MainImage == imageUrl) || 
                this.context.ProductsImages.IgnoreAllQueryFilter(hardCheck).Any(x => x.Image == imageUrl);
        }

        public async Task AddProductImageAsync(ProductImage image)
        {
            await this.context.ProductsImages.AddAsync(image);
            await this.context.SaveChangesAsync();
        }

        public async Task RemoveProductAsync(string productId)
        {
            var productToDelete = this.GetProductById(productId);
            await this.context.Entry(productToDelete).Collection(x => x.ProductCarts).LoadAsync();

            this.context.RemoveRange(productToDelete.ProductCarts);

            await this.DeleteEntityAsync(productToDelete);
        }

        public async Task EditAsync(AddProductInputModel inputModel)
        {
            var productToEdit = this.GetProductById(inputModel.Id);
            var newAdditionalImages = inputModel.AdditionalImages.Where(x => x != null).ToList();

            //Edit all of the images for the product
            this.context.ProductsImages.RemoveRange(productToEdit.AdditionalImages);

            for (int i = 0; i < newAdditionalImages.Count; i++)
            {
                var newImage = newAdditionalImages[i];
                productToEdit.AdditionalImages.Add(new ProductImage { Image = newImage, ProductId = productToEdit.Id});
            }

            //Edit all of the simple properties
            productToEdit.Description = inputModel.Description;
            productToEdit.Warranty = inputModel.Warranty;
            productToEdit.MainImage = inputModel.MainImage;
            productToEdit.Model = inputModel.Model;
            productToEdit.Name = inputModel.Name;
            productToEdit.Price = inputModel.Price;
            productToEdit.QuantityInStock = inputModel.QuantityInStock;

            await this.context.SaveChangesAsync();
        }

        public bool ProductExistsByName(string name, string excludedProductId)
        {
            return this.context.Products
                .Where(x => x.Id != excludedProductId)
                .Any(x =>x.Name == name);
        }

        public bool ProductExistsByModel(string model, string excludedProductId, bool hardCheck = false)
        {
            return this.context.Products
                .IgnoreAllQueryFilter(hardCheck)
                .Where(x => x.Id != excludedProductId)
                .Any(x => x.Model == model);
        }

        public bool ProductImageExists(string imageUrl, string excludedProductId, bool hardCheck = false)
        {
            return this.context.Products.IgnoreAllQueryFilter(hardCheck).Where(x => x.Id != excludedProductId).Any(x => x.MainImage == imageUrl) ||
                this.context.ProductsImages.IgnoreAllQueryFilter(hardCheck).Where(x => x.ProductId != excludedProductId).Any(x => x.Image == imageUrl);
        }

        public bool ImagesAreRepeated(string mainImage, List<string> additionalImages)
        {
            var additionalImagesAreSame = additionalImages.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().Count() != additionalImages.Where(x => !string.IsNullOrWhiteSpace(x)).Count();
            return additionalImagesAreSame || additionalImages.Contains(mainImage);
        }
    }
}
