using GymHub.Data.Models;
using GymHub.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GymHub.Web.Controllers
{
    public class CartsController : Controller
    {
        private readonly ICartService cartService;
        private readonly UserManager<User> userManager;
        public CartsController(ICartService cartService, UserManager<User> userManager)
        {
            this.cartService = cartService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> All()
        {
            var currentUserId = this.userManager.GetUserId(this.User);
            var productsInCart = await this.cartService.GetAllProductsFromCartAsync(currentUserId);

            return this.View(productsInCart);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Buy(List<object> inputModels)
        {
            throw new NotImplementedException("Ne sum go vkaral tova");
            return this.View("../Home/Index", inputModels);
        }

        [Authorize]
        public async Task<IActionResult> AddToCart(string productId, int quantity)
        {
            var currentUserId = this.userManager.GetUserId(this.User);
            await this.cartService.AddToCartAsync(productId, currentUserId, quantity);

            return this.Redirect("/Carts/All");
        }
    }
}
