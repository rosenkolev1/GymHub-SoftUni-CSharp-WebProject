using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using GymHub.Web.Models.ViewModels.Products.All;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.ProductService
{
    public interface IProductService
    {
        public Task AddAsync(Product product);
        public List<ProductViewModel> GetProductsFrom(int page, List<ProductFilterOptionsViewModel> productFilterOptions);
        public bool ProductExistsById(string id);
        public bool ProductExistsByName(string name);
        public bool ProductExistsByName(string name, string excludedProductId);
        public bool ProductExistsByModel(string model, bool hardCheck = false);
        public bool ProductExistsByModel(string model, string excludedProductId, bool hardCheck = false);
        public Product GetProductById(string productId, bool withNavigationalProperties);
        public double GetAverageRating(List<ProductRating> productRatings);
        public string GetShordDescription(string description, int stringLength);
        public string GetProductId(string model, bool hardCheck = false);
        public Task AddRatingAsync(string commentId, string productId, string userId, double rating);
        public Task AddRatingAsync(ProductRating productRating);
        public bool ProductRatingExists(ProductRating productRating, bool hardCheck = false);
        public ProductRating GetProductRating(string userId, string productId);
        public bool ProductRatingExists(string userId, string productId);
        public Task EditProductRating(ProductRating productRating, double rating);
        public Task RemoveProductAsync(string productId);
        public Task EditAsync(EditProductInputModel inputModel);
        public string GetProductName(string productId);
        public IQueryable<Product> FilterProducts(List<ProductFilterOptionsViewModel> productFilterOptions);
        public IQueryable<Product> FilterProducts(IQueryable<Product> products, string searchString);
        public List<ProductViewModel> GetProductsForPage(IQueryable<Product> products, int page);
        public IQueryable<Product> OrderProducts(IQueryable<Product> products, ProductOrderingOption productOrderingOption);
    }
}
