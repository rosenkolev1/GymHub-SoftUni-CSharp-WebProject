using AutoMapper;
using GymHub.Data;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            this.context.SaveChanges();
        }

        public async Task AddAsync(AddProductInputModel inputModel)
        {
            this.context.Add(mapper.Map<Product>(inputModel));
            this.context.SaveChanges();
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
    }
}
