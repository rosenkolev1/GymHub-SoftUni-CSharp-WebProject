using GymHub.Common;
using GymHub.Data.Models;
using GymHub.Services;
using GymHub.Services.ServicesFolder.CartService;
using GymHub.Services.ServicesFolder.ProductService;
using GymHub.Web.Helpers.NotificationHelpers;
using GymHub.Web.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymHub.Web.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly ICartService cartService;
        private readonly UserManager<User> userManager;
        private readonly IProductService productService;
        private readonly IUserService userService;
        public CartsController(ICartService cartService, UserManager<User> userManager, IProductService productService, IUserService userService)
        {
            this.cartService = cartService;
            this.userManager = userManager;
            this.productService = productService;
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string productId)
        {
            //Check if this product exists
            if(this.productService.ProductExistsById(productId) == false)
            {
                this.ModelState.AddModelError("", "This product doesn't exist");

                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                //Set notification
                NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "This product doesn't exist");

                return this.RedirectToAction(nameof(All));
            }

            await this.cartService.RemoveProductByIdAsync(this.userService.GetUserId(this.User.Identity.Name), productId);

            return this.RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All()
        {
            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);
            var productsInCart = this.cartService.GetAllProductsForCartViewModel(currentUserId);

            var complexModel = new ComplexModel<List<BuyProductInputModel>, List<ProductCartViewModel>>
            {
                ViewModel = productsInCart
            };

            if (TempData.ContainsKey(GlobalConstants.ErrorsFromPOSTRequest))
            {
                //Merge model states
                ModelStateHelper.MergeModelStates(TempData, this.ModelState);
            }

            if(this.TempData[GlobalConstants.InputModelFromPOSTRequestType]?.ToString() == $"List<{nameof(BuyProductInputModel)}>")
            {
                complexModel.InputModel = JsonSerializer.Deserialize<List<BuyProductInputModel>>(this.TempData[GlobalConstants.InputModelFromPOSTRequest]?.ToString());
            }

            complexModel.ViewModel
                .OrderBy(x => x.Id)
                .ToList();

            //Validate if quantity for one of the products exceeds the quantity in stock
            for (int i = 0; i < complexModel.ViewModel.Count; i++)
            {
                var productViewModel = complexModel.ViewModel[i];
                if (productViewModel.QuantityInStock < productViewModel.Quantity)
                {
                    this.ModelState.AddModelError($"InputModel[{i}].Quantity", "This product's selected quantity is more than the available quantity in stock.");
                    if (this.ViewData["CartBuyButtonErrorForQuantity"] == null) this.ViewData["CartBuyButtonErrorForQuantity"] = true;
                }
            }

            return this.View(complexModel);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(ComplexModel<List<BuyProductInputModel>, List<ProductCartViewModel>> complexModel)
        {
            this.TempData[GlobalConstants.InputModelFromPOSTRequestType] = $"List<{nameof(BuyProductInputModel)}>";
            this.TempData[GlobalConstants.InputModelFromPOSTRequest] = JsonSerializer.Serialize(complexModel.InputModel);

            //Inital model validation without checking the database
            if (this.ModelState.IsValid == false)
            {
                this.TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                //Set notification
                NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "An error occured while processing your request");

                return this.RedirectToAction(nameof(All));
            }

            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);

            //Check if the product is in the cart
            foreach (var productInputModel in complexModel.InputModel)
            {
                if (this.cartService.ProductIsInCart(productInputModel.Id, currentUserId) == false)
                {
                    //Set notification
                    NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "One or more of the products are not in the cart");

                    this.ModelState.AddModelError("", "One or more of the products are not in the cart");

                    //Set error model state
                    this.TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                    return this.RedirectToAction(nameof(All));
                }
            }

            //Validation for the quantity of a product and whether or not it exceed the quantity in stock for the product
            if (this.cartService.IsQuantityOfPurchasesValid(currentUserId, complexModel.InputModel.ToList()) == false)
            {
                //Set notification
                NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "Quantity of purchase of one or more products exceeds the quantity in stock");
            }

            if (this.ModelState.IsValid == false)
            {
                //Set error model state
                this.TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                return this.RedirectToAction(nameof(All));
            }

            var inputModel = complexModel.InputModel;

            foreach (var item in inputModel)
            {
                await this.cartService.SetCheckoutCartAsync(currentUserId, inputModel);
            }

            return this.RedirectToAction("Checkout", "Sales");
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartInputModel inputModel)
        {
            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);
            var productId = inputModel.ProductId;
            var quantity = inputModel.Quantity;
            var product = this.productService.GetProductById(productId, false);

            if(product.QuantityInStock < quantity)
            {
                this.ModelState.AddModelError("QuantityInStock", "Cannot buy more of this product than there is in stock");
            }

            if (this.ModelState.IsValid == false)
            {
                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                //Store needed info for get request in TempData
                return this.RedirectToAction("ProductPage", "Products", new { productId = productId }, "Reviews");
            }

            await this.cartService.AddToCartAsync(productId, currentUserId, quantity);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
