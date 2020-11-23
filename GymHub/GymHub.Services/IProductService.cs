using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymHub.Services
{
    public interface IProductService
    {
        public Task AddAsync(string name, string mainImage, decimal price, string description, int warranty, int quantityInStock);
        public Task AddAsync(AddProductInputModel inputModel);
        public Task AddAsync(Product product);
        public Task<List<ProductViewModel>> GetAllProductsAsync();
        public Task<bool> ProductExistsByIdAsync(string id);
        public Task<bool> ProductExistsByNameAsync(string name);
        public Task<bool> ProductExistsByModelAsync(string model);
        public Task<Product> GetProductByIdAsync(string productId);
        public double GetAverageRating(List<ProductRating> productRatings);
        public string GetShordDescription(string description, int stringLength);
        public string GetProductId(string model);
        public Task AddRatingAsync(string productId, string userId, double rating);
        public Task AddRatingAsync(ProductRating productRating);
        public bool ProductRatingExists(ProductRating productRating);
        public ProductRating GetProductRating(string userId, string productId);
        public bool ProductRatingExists(string userId, string productId);
        public Task EditProductRating(ProductRating productRating, double rating);
    }
}
