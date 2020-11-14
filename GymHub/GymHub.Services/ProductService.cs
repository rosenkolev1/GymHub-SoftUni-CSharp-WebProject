using AutoMapper;
using GymHub.Data;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ProductService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
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

        public async Task<bool> ProductExistsByIdAsync(string id)
        {
            return this.context.Products.Any(x => x.Id == id);
        }

        public async Task<bool> ProductExistsByNameAsync(string name)
        {
            return this.context.Products.Any(x => x.Name == name);
        }
        public async Task<bool> ProductExistsByModelAsync(string model)
        {
            return this.context.Products.Any(x => x.Model == model);
        }

        public async Task<List<ProductViewModel>> GetAllProductsAsync()
        {
            return this.context.Products.Select(product => mapper.Map<ProductViewModel>(product)).ToList();
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            return this.context.Products
                .Include(x => x.ProductComments)
                .Include(x => x.ProductRatings)
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
                    var neededDistance = distanceToFloor > distanceToCeiling ? distanceToCeiling : -distanceToFloor;
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
    }
}
