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
        public List<ProductViewModel> GetAllProducts();
        public bool ProductExistsById(string id);
        public bool ProductExistsByName(string name);
        public bool ProductExistsByModel(string model, bool hardCheck = false);
        public Product GetProductById(string productId);
        public double GetAverageRating(List<ProductRating> productRatings);
        public string GetShordDescription(string description, int stringLength);
        public string GetProductId(string model, bool hardCheck = false);
        public Task AddRatingAsync(string commentId, string productId, string userId, double rating);
        public Task AddRatingAsync(ProductRating productRating);
        public bool ProductRatingExists(ProductRating productRating, bool hardCheck = false);
        public ProductRating GetProductRating(string userId, string productId);
        public bool ProductRatingExists(string userId, string productId);
        public Task EditProductRating(ProductRating productRating, double rating);
        public bool ProductImageExists(string imageUrl, bool hardCheck = false);
        public Task AddProductImageAsync(ProductImage image);
        public Task RemoveProductAsync(string productId);
    }
}
