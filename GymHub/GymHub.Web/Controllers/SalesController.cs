using AutoMapper;
using GymHub.Common;
using GymHub.Data.Models;
using GymHub.Services;
using GymHub.Services.ServicesFolder.CartService;
using GymHub.Services.ServicesFolder.CountryService;
using GymHub.Services.ServicesFolder.PaymentMethodService;
using GymHub.Services.ServicesFolder.ProductService;
using GymHub.Services.ServicesFolder.SaleService;
using GymHub.Web.Helpers.NotificationHelpers;
using GymHub.Web.Models;
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
        private readonly IMapper mapper;

        public SalesController
            (ISaleService saleService, IProductService productService, ICountryService countryService, IPaymentMethodService paymentMethodService, ICartService cartService, IUserService userService, IMapper mapper)
        {
            this.saleService = saleService;
            this.productService = productService;
            this.countryService = countryService;
            this.paymentMethodService = paymentMethodService;
            this.cartService = cartService;
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Checkout()
        {
            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);

            var cardProducts = this.cartService.GetAllProductsFromCart(currentUserId);

            //Check if the user has any products in their cart
            if (cardProducts.Count == 0)
            {
                //Set notification
                NotificationHelper.SetNotification(TempData, NotificationType.Error, "You do not have any products in your cart");

                return this.RedirectToAction("All", "Carts");
            }

            var checkoutViewModel = new CheckoutViewModel
            {
                ProductsInfo = cardProducts
                .Select(x => new CheckoutProductViewModel
                {
                    Id = x.Id,
                    Quantity = x.Quantity,
                    SinglePrice = x.Price,
                    Name = this.productService.GetProductName(x.Id),
                    Model = this.productService.GetProductModel(x.Id)
                }).ToList(),
                TotalPrice = cardProducts.Sum(x => x.Price * x.Quantity),
                Countries = this.countryService.GetAllCountries(),
                PaymentMethods = this.paymentMethodService.GetAllPaymentMethods()
            };

            var complexModel = new ComplexModel<CheckoutInputModel, CheckoutViewModel>
            {
                ViewModel = checkoutViewModel
            };

            //TODO: Add input model on error from post request

            if(this.TempData[GlobalConstants.ErrorsFromPOSTRequest] != null)
            {
                ModelStateHelper.MergeModelStates(this.TempData, this.ModelState);
            }

            if(this.TempData[GlobalConstants.InputModelFromPOSTRequestType]?.ToString() == nameof(CheckoutInputModel))
            {
                complexModel.InputModel = JsonSerializer.Deserialize<CheckoutInputModel>(this.TempData[GlobalConstants.InputModelFromPOSTRequest]?.ToString());
            }

            return this.View(complexModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(ComplexModel<CheckoutInputModel, CheckoutViewModel> complexModel)
        {


            this.TempData[GlobalConstants.InputModelFromPOSTRequest] = JsonSerializer.Serialize(complexModel.InputModel);
            this.TempData[GlobalConstants.InputModelFromPOSTRequestType] = nameof(CheckoutInputModel);

            //Validate model without checking the database
            if(this.ModelState.IsValid == false)
            {
                //Serialize errors from modelstate
                this.TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                //Set notification
                NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "An error occured while processing your request");

                return this.RedirectToAction(nameof(Checkout));
            }

            //Check if the payment method exists
            if(this.paymentMethodService.PaymentMethodExistsById(complexModel.InputModel.PaymentMethodId) == false)
            {
                this.ModelState.AddModelError("", "This payment method doesn't exist");

                //Serialize errors from modelstate
                this.TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                //Set notification
                NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "Selected payment method doesn't exist");

                return this.RedirectToAction(nameof(Checkout));
            }

            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);

            var cartProducts = this.cartService.GetAllProductsFromCart(currentUserId);

            var inputModel = complexModel.InputModel;

            //Check if the user has any products in their cart
            if (await this.cartService.GetNumberOfProductsInCart(currentUserId) == 0)
            {
                //Set notification
                NotificationHelper.SetNotification(TempData, NotificationType.Error, "You do not have any products in your cart");

                return this.RedirectToAction("All", "Carts");
            }

            //Check if the products' quantities exceed the amount of units in stock
            foreach (var product in cartProducts)
            {
                if(product.Quantity > product.QuantityInStock)
                {
                    this.ModelState.AddModelError("", $"{product.Name}:{product.Model} has less units available than you are currently trying to buy");

                    NotificationHelper.SetNotification(this.TempData, NotificationType.Error, $"{product.Name}:{product.Model} has less units available than you are currently trying to buy");
                }
            }

            if(this.ModelState.IsValid == false)
            {
                //Serialize errors from modelstate
                this.TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                return this.RedirectToAction(nameof(Checkout));
            }

            await this.saleService.CheckoutAsync(inputModel, currentUserId, cartProducts);

            NotificationHelper.SetNotification(this.TempData, NotificationType.Success, "You successfully made a purchase. You can review your purchase information in the profile tab");

            return this.RedirectToAction("All", "Carts");
        }

    }
}
