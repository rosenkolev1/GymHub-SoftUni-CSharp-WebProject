using GymHub.Data.Models;
using GymHub.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public interface ICartService
    {
        public Task AddToCartAsync(string productId, string userId, int quantity = 1);
        public Task<bool> ProductIsInCartAsync(string productId, string userId);
        public Task<ProductCart> GetProductFromCartAsync(string productId, string userId);
        public Task<List<ProductCartViewModel>> GetAllProductsFromCartAsync(string userId);
    }
}
