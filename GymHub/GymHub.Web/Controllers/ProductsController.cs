using AutoMapper;
using GymHub.Data.Models;
using GymHub.Services;
using GymHub.Web.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using GymHub.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace GymHub.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly IUserService userService;
        private readonly IProductCommentService productCommentService;
        private readonly IMapper mapper;
        public ProductsController(IProductService productService, IMapper mapper, IProductCommentService productCommentService, IUserService userService)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.productCommentService = productCommentService;
            this.userService = userService;
        }

        [Authorize]
        public async Task<IActionResult> All()
        {
            var products = await this.productService.GetAllProductsAsync();
            foreach (var product in products)
            {
                product.ShortDescription = this.productService.GetShordDescription(product.Description, 40);
            }
            return this.View(products);
        }

        [Authorize]
        public async Task<IActionResult> ProductPage(string productId)
        {
            var product = await this.productService.GetProductByIdAsync(productId);
            var viewModel = mapper.Map<ProductInfoViewModel>(product);

            //Short description
            viewModel.ShortDescription = this.productService.GetShordDescription(viewModel.Description, 40);

            //Overall product rating
            viewModel.ProductRating = new ProductRatingViewModel(this.productService.GetAverageRating(viewModel.ProductRatings));

            //Product rating for each user
            foreach (var userProductRating in viewModel.ProductRatings)
            {
                viewModel.UsersProductRatings.Add(userProductRating.User, new ProductRatingViewModel(userProductRating.Rating));
            }

            //Product comments
            foreach (var comment in viewModel.ProductComments)
            {
                if(comment.ParentCommentId == null)
                {
                    var childComments = await this.productCommentService.GetAllChildCommentsAsync(comment);
                    viewModel.ParentsChildrenComments.Add(comment, childComments);
                }
            }
            viewModel.ParentsChildrenComments.OrderBy(kv => kv.Key.CommentedOn);
            foreach (var kv in viewModel.ParentsChildrenComments)
            {
                var childrenComments = kv.Value;
                childrenComments.OrderBy(x => x.CommentedOn);
            }

            var complexModel = new ComplexModel<AddReviewInputModel, ProductInfoViewModel>();
            complexModel.ViewModel = viewModel;

            //Add each model state error from the last action to this one
            if(TempData["ErrorsFromPOSTRequest"] != null)
            {
                var postRequestModelState = ModelStateHelper.DeserialiseModelState(TempData["ErrorsFromPOSTRequest"].ToString());
                this.ModelState.Merge(postRequestModelState);
            }

            //Add input model sthe last post action to this one
            if (TempData["InputModelFromPOSTRequest"] != null)
            {
                var inputModel = JsonSerializer.Deserialize<AddReviewInputModel>(TempData["InputModelFromPOSTRequest"].ToString());
                complexModel.InputModel = inputModel;
            }
            
            return this.View("ProductPage", complexModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview(ComplexModel<AddReviewInputModel, ProductInfoViewModel> complexModel, string pageFragment)
        {
            var productId = complexModel.InputModel.ProductId;

            //Check if data is valid without looking into the database
            if (this.ModelState.IsValid == false)
            {
                //Store needed info for get request in TempData
                TempData["InputModelFromPOSTRequest"] = JsonSerializer.Serialize(complexModel.InputModel);
                return this.RedirectToAction(nameof(ProductPage), "Products", new { productId = productId }, pageFragment);
            }

            var userId = this.userService.GetUserId(this.User.Identity.Name);

            var newProductRating = new ProductRating
            {
                ProductId = productId,
                UserId = userId,
                Rating = (int)complexModel.InputModel.Rating
            };

            var newComment = new ProductComment
            {
                CommentedOn = DateTime.UtcNow,
                ParentCommentId = null,
                ProductId = productId,
                Text = complexModel.InputModel.Text,
                UserId = userId,
            };

            //Check if user has alread left a review for this product
            if(this.productService.ProductRatingExists(newProductRating) == true && await this.productCommentService.CommentExists(newComment) == true)
            {
                this.ModelState.AddModelError("InputModel.Rating", "You have already given a review for this product!!!");
            }
            //Check if rating from this user for this product already exists
            else if (this.productService.ProductRatingExists(newProductRating) == true)
            {
                this.ModelState.AddModelError("InputModel.Rating", "You have already given a review with this rating for this product!!!");
            }
            //Check if comment already from this user for this product already exists(as part of his review, he can still have comments as replies to other people for the same product)
            else if (await this.productCommentService.CommentExists(newComment) == true)
            {
                this.ModelState.AddModelError("InputModel.Text", "You have already given a review with a comment for this product!!!");
            }


            //Check if model state is valid after checking into the database
            if (this.ModelState.IsValid == false)
            {
                //Store needed info for get request in TempData
                TempData["ErrorsFromPOSTRequest"] = ModelStateHelper.SerialiseModelState(this.ModelState);
                TempData["InputModelFromPOSTRequest"] = JsonSerializer.Serialize(complexModel.InputModel);

                //Reload same page with the TempData
                return this.RedirectToAction(nameof(ProductPage), "Products", new { productId = productId}, pageFragment);
            }

            await this.productService.AddRatingAsync(productId, userId, (int)complexModel.InputModel.Rating);

            await this.productCommentService.AddAsync(newComment);

            return this.RedirectToAction(nameof(ProductPage), "Products", new { productId = productId }, pageFragment);
        }
    }
}
