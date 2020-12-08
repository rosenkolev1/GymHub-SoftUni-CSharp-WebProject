using GymHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services
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
    }
}
