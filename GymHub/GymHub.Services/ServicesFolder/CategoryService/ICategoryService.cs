using GymHub.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.CategoryService
{
    public interface ICategoryService
    {
        public List<Category> GetAllCategories(bool hardCheck = false);
        public bool CategoryNameExists(string name, bool hardCheck = false);
        public bool CategoryNameExists(string name, string excludedCategoryId, bool hardCheck = false);
        public Task AddAsync(string name);
        public Task AddAsync(Category category);
        public bool CategoryExists(string Id, bool hardCheck = false);
        public Task EditAsync(string id, string name);
        public Category GetCategoryById(string id);
        public Task RemoveAsync(string id);
        public Task RestoreAsync(string id);
        public Task AddCategoriesToProductAsync(Product product, List<string> productCategoriesIds);
        public List<Category> GetCategoriesForProduct(string productId);
        public Task EditCategoriesToProductAsync(Product product, List<string> productCategoriesIds);
        public List<Product> GetProductsForCategory(string categoryId);
        public List<ProductCategory> GetAllCategoriesByIds(List<string> ids);
    }
}
