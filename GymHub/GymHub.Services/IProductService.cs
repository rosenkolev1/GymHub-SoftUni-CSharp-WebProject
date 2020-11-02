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
        public Task<List<ProductViewModel>> GetAllProductsAsync();
        public Task<bool> ProductExistsAsync(string id);
    }
}
