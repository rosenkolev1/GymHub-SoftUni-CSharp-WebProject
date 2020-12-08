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

        public List<Category> GetAllCategories(bool hardCheck = false)
        {
            return this.context.Categories.IgnoreAllQueryFilter(hardCheck).ToList();
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
