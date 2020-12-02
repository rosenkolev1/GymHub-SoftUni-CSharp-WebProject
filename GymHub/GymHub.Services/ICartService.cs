using GymHub.Data.Models;
using GymHub.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymHub.Services
{
    public interface ICartService
    {
        public Task AddToCartAsync(string productId, string userId, int quantity = 1);
        public bool ProductIsInCart(string productId, string userId);
        public ProductCart GetProductFromCart(string productId, string userId);
        public List<ProductCartViewModel> GetAllProductsFromCart(string userId);
        public int GetNumberOfProductsInCart(string userId);
    }
}
