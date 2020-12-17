﻿using GymHub.Services;
using GymHub.Services.ServicesFolder.CartService;
using Microsoft.AspNetCore.Mvc;
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


        public IViewComponentResult Invoke()
        {
            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);
            int numberOfItemsInCartForCurrentUser = this.cartService.GetNumberOfProductsInCart(currentUserId);

            return this.View(numberOfItemsInCartForCurrentUser);
        }
    }
}
