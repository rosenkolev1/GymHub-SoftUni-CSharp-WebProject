using GymHub.Data.Models;
using GymHub.Web.Models.ViewModels;
using System.Collections.Generic;

namespace GymHub.Web.Services
{
    public interface ICartService
    {
        public void AddToCart(string productId, string userId, int quantity = 1);
        public bool ProductIsInCart(string productId, string userId);
        public ProductCart GetProductFromCart(string productId, string userId);
        public List<ProductCartViewModel> GetAllProductsFromCart(string userId);
    }
}
