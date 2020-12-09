using AutoMapper;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.CartService
{
    public class CartService : DeleteableEntityService, ICartService
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public CartService(ApplicationDbContext context, IMapper mapper, IUserService userService)
            : base(context)
        {
            this.mapper = mapper;
            this.userService = userService;
        }
        public async Task AddToCartAsync(string productId, string userId, int quantity = 1)
        {
            if (this.ProductIsInCart(productId, userId))
            {
                var productCart = GetProductFromCart(productId, userId);
                productCart.Quantity += quantity;
            }
            else
            {
                await context.Carts.AddAsync(new ProductCart { UserId = userId, ProductId = productId, Quantity = quantity });
            }
            await context.SaveChangesAsync();
        }

        public bool ProductIsInCart(string productId, string userId)
        {
            return context.Carts.Any(x => x.ProductId == productId && x.UserId == userId);
        }

        public ProductCart GetProductFromCart(string productId, string userId)
        {
            return context.Carts.FirstOrDefault(x => x.ProductId == productId && x.UserId == userId);
        }

        public List<ProductCartViewModel> GetAllProductsFromCart(string userId)
        {
            return context.Carts.Where(x => x.UserId == userId).Select(x => new ProductCartViewModel
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

        public async Task<int> GetNumberOfProductsInCart(string userId)
        {
            var countOfProducts = context.Carts
                .Select(x => new
                {
                    x.UserId,
                    x.Product
                })
                .Where(x => x.UserId == userId && x.Product.IsDeleted == false)
                .Count();
            return countOfProducts;
        }

        public async Task RemoveProductById(string userId, string productId)
        {
            context.Carts.Remove(GetProductFromCart(productId, userId));
            await context.SaveChangesAsync();
        }

        public async Task SetCheckoutCartAsync(string userId, List<BuyProductInputModel> inputModels)
        {
            var user = this.userService.GetUser(userId);
            await this.context.Entry(user).Collection(x => x.ProductsCart).LoadAsync();

            user.ProductsCart.Clear();

            foreach (var productItem in inputModels)
            {
                user.ProductsCart.Add(new ProductCart { UserId = userId, ProductId = productItem.Id, Quantity = productItem.Quantity });
            }

            await this.context.SaveChangesAsync();
        }
    }
}
