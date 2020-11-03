using GymHub.Data;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext context;
        public CartService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task AddToCartAsync(string productId, string userId, int quantity = 1)
        {
            if (await this.ProductIsInCartAsync(productId, userId))
            {
                var productCart = await this.GetProductFromCartAsync(productId, userId);
                productCart.Quantity += quantity;
            }
            else
            {
                this.context.Carts.Add(new ProductCart { UserId = userId, ProductId = productId, Quantity = quantity });
            }
            this.context.SaveChanges();
        }

        public async Task<bool> ProductIsInCartAsync(string productId, string userId)
        {
            return this.context.Carts.Any(x => x.ProductId == productId && x.UserId == userId);
        }

        public async Task<ProductCart> GetProductFromCartAsync(string productId, string userId)
        {
            return this.context.Carts.FirstOrDefault(x => x.ProductId == productId && x.UserId == userId);
        }

        public async Task<List<ProductCartViewModel>> GetAllProductsFromCartAsync(string userId)
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
