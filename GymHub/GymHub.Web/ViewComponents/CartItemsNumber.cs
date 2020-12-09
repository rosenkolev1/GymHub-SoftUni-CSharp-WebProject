using GymHub.Services;
using GymHub.Services.ServicesFolder.CartService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.ViewComponents
{
    [ViewComponent(Name = "CartItemsNumber")]
    public class CartItemsNumber : ViewComponent
    {
        private readonly ICartService cartService;
        private readonly IUserService userService;
        public CartItemsNumber(ICartService cartService, IUserService userService)
        {
            this.cartService = cartService;
            this.userService = userService;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);
            int numberOfItemsInCartForCurrentUser = await this.cartService.GetNumberOfProductsInCart(currentUserId);

            return this.View(numberOfItemsInCartForCurrentUser);
        }
    }
}
