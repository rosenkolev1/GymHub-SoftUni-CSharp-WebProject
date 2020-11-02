using GymHub.Data.Models;
using GymHub.Web.Data;
using GymHub.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace GymHub.Web.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext context;
        public CartService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void AddToCart(string productId, string userId, int quantity = 1)
        {
            if (this.ProductIsInCart(productId, userId))
            {
                var productCart = this.GetProductFromCart(productId, userId);
                productCart.Quantity += quantity;
            }
            else
            {
                this.context.Carts.Add(new ProductCart { UserId = userId, ProductId = productId, Quantity = quantity });
            }
            this.context.SaveChanges();
        }

        public bool ProductIsInCart(string productId, string userId)
        {
            return this.context.Carts.Any(x => x.ProductId == productId && x.UserId == userId);
        }

        public ProductCart GetProductFromCart(string productId, string userId)
        {
            return this.context.Carts.FirstOrDefault(x => x.ProductId == productId && x.UserId == userId);
        }

        public List<ProductCartViewModel> GetAllProductsFromCart(string userId)
        {
            return this.context.Carts.Where(x => x.UserId == userId).Select(x => new ProductCartViewModel
            {
                MainImage = x.Product.MainImage,
                Name = x.Product.Name,
                Price = x.Product.Price,
                Id = x.Product.Id,
                Quantity = x.Quantity
            }).ToList();
        }
    }
}
