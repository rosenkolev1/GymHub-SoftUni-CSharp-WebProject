using GymHub.Data.Models;
using GymHub.Services;
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
    public class CartsController : Controller
    {
        private readonly ICartService cartService;
        private readonly UserManager<User> userManager;
        private readonly IProductService productService;
        public CartsController(ICartService cartService, UserManager<User> userManager, IProductService productService)
        {
            this.cartService = cartService;
            this.userManager = userManager;
            this.productService = productService;
        }

        [Authorize]
        public async Task<IActionResult> All()
        {
            var currentUserId = this.userManager.GetUserId(this.User);
            var productsInCart = this.cartService.GetAllProductsFromCart(currentUserId);

            var complexModel = new ComplexModel<List<BuyProductInputModel>, List<ProductCartViewModel>>
            {
                ViewModel = productsInCart
            };

            return this.View(complexModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(ComplexModel<List<BuyProductInputModel>, List<ProductCartViewModel>> complexModel)
        {
            var inputModel = complexModel.InputModel;
            var totalPriceOfAllProducts = inputModel.Sum(x => x.Quantity * x.SinglePrice);
            return this.Redirect("/");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartInputModel inputModel)
        {
            var currentUserId = this.userManager.GetUserId(this.User);
            var productId = inputModel.ProductId;
            var quantity = inputModel.Quantity;
            var product = this.productService.GetProductById(productId);

            //Store input model for passing in get action
            //TempData["InputModelFromPOSTRequest"] = JsonSerializer.Serialize(new { productId = productId, quantity = quantity });
            //TempData["InputModelFromPOSTRequestType"] = "AddToCartInputModel";

            if(product.QuantityInStock < quantity)
            {
                this.ModelState.AddModelError("QuantityInStock", "Cannot buy more of this product than there is in stock");
            }

            if (this.ModelState.IsValid == false)
            {
                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData["ErrorsFromPOSTRequest"] = ModelStateHelper.SerialiseModelState(this.ModelState);

                //Store needed info for get request in TempData
                return this.RedirectToAction("ProductPage", "Products", new { productId = productId }, "Reviews");
                //return this.RedirectToRoute($"/Products/ProductPage?productId={productId}", new { productId = productId }, "Reviews");
            }

            await this.cartService.AddToCartAsync(productId, currentUserId, quantity);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
