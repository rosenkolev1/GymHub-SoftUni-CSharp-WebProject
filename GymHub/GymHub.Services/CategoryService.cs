using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services
{
    public class CategoryService : DeleteableEntityService, ICategoryService
    {
        public CategoryService(ApplicationDbContext context)
            : base(context)
        {
            
        }

        public async Task AddAsync(string name)
        {
            await this.context.Categories.AddAsync(new Category { Name = name });
            await this.context.SaveChangesAsync();
        }

        public async Task AddAsync(Category category)
        {
            await this.context.Categories.AddAsync(category);
            await this.context.SaveChangesAsync();
        }

        public async Task AddCategoriesToProductAsync(Product product, List<string> productCategoriesIds)
        {
            if(product.ProductCategories == null) product.ProductCategories = new List<ProductCategory>();
            foreach (var categoryId in productCategoriesIds)
            {
                if(product.ProductCategories.Any(x => x.CategoryId == categoryId) == false)
                {
                    product.ProductCategories.Add(new ProductCategory { CategoryId = categoryId, ProductId = product.Id });
                }
            }

            await this.context.SaveChangesAsync();
        }

        public bool CategoryExists(string Id, bool hardCheck = false)
        {
            return this.context.Categories.IgnoreAllQueryFilter(hardCheck).Any(x => x.Id == Id);
        }

        public bool CategoryNameExists(string name, bool hardCheck = false)
        {
            return this.context.Categories.IgnoreAllQueryFilter(hardCheck).Any(x => x.Name == name);
        }

        public bool CategoryNameExists(string name, string excludedCategoryId, bool hardCheck = false)
        {
            return this.context.Categories.IgnoreAllQueryFilter(hardCheck).Any(x => x.Name == name && x.Id != excludedCategoryId);
        }

        public async Task EditAsync(string id, string name)
        {
            var category = this.context.Categories.First(x => x.Id == id);
            category.Name = name;
            await this.context.SaveChangesAsync();
        }

        public async Task EditCategoriesToProductAsync(Product product, List<string> productCategoriesId)
        {
            var productCategories = this.context.ProductsCategories
                .IgnoreAllQueryFilter(true).Where(x => x.Product == product).ToList();

            //Delete the old categories
            foreach (var category in productCategories.Where(x => productCategoriesId.Contains(x.Id) == false))
            {
                category.IsDeleted = true;
                category.DeletedOn = DateTime.UtcNow;
            }

            //Add the new categories
            foreach (var categoryId in productCategoriesId)
            {
                var productCategory = productCategories.FirstOrDefault(x => x.CategoryId == categoryId);
                if (productCategory == null)
                {
                    product.ProductCategories.Add(new ProductCategory { CategoryId = categoryId, ProductId = product.Id });
                }
                else
                {
                    productCategory.IsDeleted = false;
                    productCategory.DeletedOn = null;
                }
            }

            await this.context.SaveChangesAsync();
        }

        public List<Category> GetAllCategories(bool hardCheck = false)
        {
            return this.context.Categories.IgnoreAllQueryFilter(hardCheck).ToList();
        }

        public List<Category> GetCategoriesForProduct(string productId)
        {
            return this.context.ProductsCategories.Where(x => x.ProductId == productId)
                .Select(x => x.Category).ToList();
        }

        public Category GetCategoryById(string id)
        {
            return this.context.Categories.First(x => x.Id == id);
        }

        public async Task RemoveAsync(string id)
        {
            await this.DeleteEntityAsync(this.GetCategoryById(id));
        }

        public async Task RestoreAsync(string id)
        {
            var category = this.context.Categories.IgnoreAllQueryFilter(true).First(x => x.Id == id);
            category.IsDeleted = false;
            category.DeletedOn = null;
            await this.context.SaveChangesAsync();
        }
    }
}
