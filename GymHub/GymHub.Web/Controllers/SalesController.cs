using GymHub.Common;
using GymHub.Services;
using GymHub.Services.ServicesFolder.CartService;
using GymHub.Services.ServicesFolder.CountryService;
using GymHub.Services.ServicesFolder.PaymentMethodService;
using GymHub.Services.ServicesFolder.ProductService;
using GymHub.Services.ServicesFolder.SaleService;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymHub.Web.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private readonly ISaleService saleService;
        private readonly IProductService productService;
        private readonly ICountryService countryService;
        private readonly IPaymentMethodService paymentMethodService;
        private readonly ICartService cartService;
        private readonly IUserService userService;

        public SalesController
            (ISaleService saleService, IProductService productService, ICountryService countryService, IPaymentMethodService paymentMethodService, ICartService cartService, IUserService userService)
        {
            this.saleService = saleService;
            this.productService = productService;
            this.countryService = countryService;
            this.paymentMethodService = paymentMethodService;
            this.cartService = cartService;
            this.userService = userService;
        }

        public async Task<IActionResult> Checkout()
        {
            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);

            var cardProducts = this.cartService.GetAllProductsFromCart(currentUserId);

            var checkoutViewModel = new CheckoutViewModel
            {
                ProductsInfo = cardProducts
                .Select(x => new CheckoutProductViewModel
                {
                    Id = x.Id,
                    Quantity = x.Quantity,
                    SinglePrice = x.Price,
                    Name = this.productService.GetProductName(x.Id),
                }).ToList(),
                TotalPrice = cardProducts.Sum(x => x.Price * x.Quantity),
                Countries = this.countryService.GetAllCountries(),
                PaymentMethods = this.paymentMethodService.GetAllPaymentMethods()
            };

            return this.View(checkoutViewModel);
        } 
    }
}
