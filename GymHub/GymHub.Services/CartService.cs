using AutoMapper;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using GymHub.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Services
{
    public class CartService : DeleteableEntityService, ICartService
    {
        private readonly IMapper mapper;

        public CartService(ApplicationDbContext context, IMapper mapper)
            :base(context)
        {
            this.mapper = mapper;
        }
        public async Task AddToCartAsync(string productId, string userId, int quantity = 1)
        {
            if (this.ProductIsInCart(productId, userId))
            {
                var productCart = this.GetProductFromCart(productId, userId);
                productCart.Quantity += quantity;
            }
            else
            {
                await this.context.Carts.AddAsync(new ProductCart { UserId = userId, ProductId = productId, Quantity = quantity });
            }
            await this.context.SaveChangesAsync();
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
                Quantity = x.Quantity,
                Model = x.Product.Model,
                QuantityInStock = x.Product.QuantityInStock
            }).ToList();
        }
    }
}
