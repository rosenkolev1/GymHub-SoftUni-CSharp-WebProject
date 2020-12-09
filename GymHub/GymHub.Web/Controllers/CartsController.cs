using GymHub.Common;
using GymHub.Data.Models;
using GymHub.Services;
using GymHub.Services.ServicesFolder.CartService;
using GymHub.Services.ServicesFolder.ProductService;
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

                return this.RedirectToAction(nameof(All));
            }

            await this.cartService.RemoveProductById(this.userService.GetUserId(this.User.Identity.Name), productId);

            return this.RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All()
        {
            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);
            var productsInCart = this.cartService.GetAllProductsFromCart(currentUserId);

            var complexModel = new ComplexModel<List<BuyProductInputModel>, List<ProductCartViewModel>>
            {
                ViewModel = productsInCart
            };

            if (TempData.ContainsKey(GlobalConstants.ErrorsFromPOSTRequest))
            {
                //Merge model states
                ModelStateHelper.MergeModelStates(TempData, this.ModelState);
            }

            return this.View(complexModel);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(ComplexModel<List<BuyProductInputModel>, List<ProductCartViewModel>> complexModel)
        {
            //TODO: ADD validations
            var inputModel = complexModel.InputModel;

            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);

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
