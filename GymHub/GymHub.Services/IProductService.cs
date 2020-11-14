using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymHub.Web.Services
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
    }
}
