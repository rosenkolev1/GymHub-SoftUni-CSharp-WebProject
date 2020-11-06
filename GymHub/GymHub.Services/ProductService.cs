using GymHub.Data;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;
        public ProductService(ApplicationDbContext context)
        {
            this.context = context;
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
            this.context.Add(new Product
            {
                Name = inputModel.Name,
                MainImage = inputModel.MainImage,
                Price = inputModel.Price,
                Description = inputModel.Description,
                Warranty = inputModel.Warranty,
                QuantityInStock = inputModel.QuantityInStock,
                Model = inputModel.Model,
                IsDeleted = false,
                DeletedOn = null
            });
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
            return this.context.Products.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                QuantityInStock = x.QuantityInStock,
                Description = x.Description,
                MainImage = x.MainImage,
                Price = x.Price,
                Warranty = x.Warranty
            }).ToList();
        }
    }
}
