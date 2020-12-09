using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.CategoryService
{
    public class CategoryService : DeleteableEntityService, ICategoryService
    {

        public CategoryService(ApplicationDbContext context)
            : base(context)
        {

        }

        public async Task AddAsync(string name)
        {
            await context.Categories.AddAsync(new Category { Name = name });
            await context.SaveChangesAsync();
        }

        public async Task AddAsync(Category category)
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
        }

        public async Task AddCategoriesToProductAsync(Product product, List<string> productCategoriesIds)
        {
            if (product.ProductCategories == null) product.ProductCategories = new List<ProductCategory>();
            foreach (var categoryId in productCategoriesIds)
            {
                if (product.ProductCategories.Any(x => x.CategoryId == categoryId) == false)
                {
                    product.ProductCategories.Add(new ProductCategory { CategoryId = categoryId, ProductId = product.Id });
                }
            }

            await context.SaveChangesAsync();
        }

        public bool CategoryExists(string Id, bool hardCheck = false)
        {
            return context.Categories.IgnoreAllQueryFilters(hardCheck).Any(x => x.Id == Id);
        }

        public bool CategoryNameExists(string name, bool hardCheck = false)
        {
            return context.Categories.IgnoreAllQueryFilters(hardCheck).Any(x => x.Name == name);
        }

        public bool CategoryNameExists(string name, string excludedCategoryId, bool hardCheck = false)
        {
            return context.Categories.IgnoreAllQueryFilters(hardCheck).Any(x => x.Name == name && x.Id != excludedCategoryId);
        }

        public async Task EditAsync(string id, string name)
        {
            var category = context.Categories.First(x => x.Id == id);
            category.Name = name;
            await context.SaveChangesAsync();
        }

        public async Task EditCategoriesToProductAsync(Product product, List<string> productCategoriesId)
        {
            var productCategories = context.Products
                .Where(x => x == product)
                .Select(x => x.ProductCategories)
                .FirstOrDefault()
                .ToList();


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
                    if (context.Entry(product).Collection(x => x.ProductCategories).IsLoaded == false)
                    {
                        await context.Entry(product).Collection(x => x.ProductCategories).LoadAsync();
                    }

                    product.ProductCategories.Add(new ProductCategory { CategoryId = categoryId, ProductId = product.Id });
                }
                else
                {
                    productCategory.IsDeleted = false;
                    productCategory.DeletedOn = null;
                }
            }

            await context.SaveChangesAsync();
        }

        public List<Category> GetAllCategories(bool hardCheck = false)
        {
            return context.Categories.IgnoreAllQueryFilters(hardCheck).ToList();
        }

        public List<Category> GetCategoriesForProduct(string productId)
        {
            var productCategories = context.Products
                .Where(x => x.Id == productId)
                .SelectMany(x => x.ProductCategories)
                .Select(x => x.Category)
                .ToList();

            return productCategories;
        }

        public Category GetCategoryById(string id)
        {
            return context.Categories.First(x => x.Id == id);
        }

        public List<Product> GetProductsForCategory(string categoryId)
        {
            var products = context.Products
                .Select(x => new { Product = x, x.ProductCategories })
                .Where(pc => pc.ProductCategories.Any(x => x.CategoryId == categoryId))
                .Select(x => x.Product)
                .ToList();

            return products;
        }

        public async Task RemoveAsync(string id)
        {
            await DeleteEntityAsync(GetCategoryById(id));
        }

        public async Task RestoreAsync(string id)
        {
            var category = context.Categories.IgnoreAllQueryFilters(true).First(x => x.Id == id);
            category.IsDeleted = false;
            category.DeletedOn = null;
            await context.SaveChangesAsync();
        }
    }
}
