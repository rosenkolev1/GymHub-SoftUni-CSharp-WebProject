using AutoMapper;
using GymHub.Data.Models;
using GymHub.Services;
using GymHub.Web.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using GymHub.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Text.Encodings.Web;
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
        private readonly JavaScriptEncoder javaScriptEncoder;
        public ProductsController(IProductService productService, IMapper mapper, IProductCommentService productCommentService, IUserService userService, JavaScriptEncoder javaScriptEncoder)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.productCommentService = productCommentService;
            this.userService = userService;
            this.javaScriptEncoder = javaScriptEncoder;
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

            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);
            viewModel.CurrentUserId = currentUserId;

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
                if (comment.ParentCommentId == null)
                {
                    var childComments = await this.productCommentService.GetAllChildCommentsAsync(comment);
                    viewModel.ParentsChildrenComments.Add(comment, childComments);
                }
            }

            //Order comments and all of it's replies
            viewModel.ParentsChildrenComments.OrderBy(kv => kv.Key.CommentedOn);
            foreach (var kv in viewModel.ParentsChildrenComments)
            {
                var childrenComments = kv.Value;
                childrenComments.OrderBy(x => x.CommentedOn);
            }

            //Check if user has already given a review on this product
            viewModel.ReviewedByCurrentUser = viewModel.ParentsChildrenComments.Keys.Any(x => x.UserId == currentUserId);

            //Add each model state error from the last action to this one
            if (TempData["ErrorsFromPOSTRequest"] != null)
            {
                var postRequestModelState = ModelStateHelper.DeserialiseModelState(TempData["ErrorsFromPOSTRequest"].ToString());
                this.ModelState.Merge(postRequestModelState);
            }

            object complexModel = null;
            var typeOfInputModel = TempData["InputModelFromPOSTRequestType"];

            if(typeOfInputModel?.ToString() == nameof(AddReviewInputModel))
            {
                complexModel = AssignViewAndInputModels<AddReviewInputModel, ProductInfoViewModel>(viewModel);
            }
            else
            {
                complexModel = AssignViewAndInputModels<AddReviewInputModel, ProductInfoViewModel>(viewModel, true);
            }

            return this.View("ProductPage", complexModel);
        }

        public ComplexModel<InputModelType, ViewModelType> AssignViewAndInputModels<InputModelType, ViewModelType>(ViewModelType viewModel, bool onlyViewModel = false)
        {
            //Asign view model to complex model
            var complexModel = new ComplexModel<InputModelType, ViewModelType>();
            complexModel.ViewModel = viewModel;

            if (onlyViewModel) return complexModel;

            //Get inputModel from TempDate
            var inputModelJSON = TempData["InputModelFromPOSTRequest"]?.ToString();

            //Add input model the last post action to this one
            if (inputModelJSON != null)
            {
                var inputModel = JsonSerializer.Deserialize<InputModelType>(inputModelJSON);
                complexModel.InputModel = inputModel;
            }

            return complexModel;
        } 

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview(ComplexModel<AddReviewInputModel, ProductInfoViewModel> complexModel, string pageFragment)
        {
            var productId = complexModel.InputModel.ProductId;

            //Store input model for passing in get action
            TempData["InputModelFromPOSTRequest"] = JsonSerializer.Serialize(complexModel.InputModel);
            TempData["InputModelFromPOSTRequestType"] = nameof(AddReviewInputModel);

            //Check if data is valid without looking into the database
            if (this.ModelState.IsValid == false)
            {
                //Store needed info for get request in TempData
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
            if (this.productService.ProductRatingExists(newProductRating) == true && await this.productCommentService.CommentExists(newComment) == true)
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
                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData["ErrorsFromPOSTRequest"] = ModelStateHelper.SerialiseModelState(this.ModelState);

                //Reload same page with the TempData
                return this.RedirectToAction(nameof(ProductPage), "Products", new { productId = productId }, pageFragment);
            }

            await this.productService.AddRatingAsync(productId, userId, (int)complexModel.InputModel.Rating);

            await this.productCommentService.AddAsync(newComment);

            return this.RedirectToAction(nameof(ProductPage), "Products", new { productId = productId }, pageFragment);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditReview(EditReviewInputModel inputModel, string pageFragment)
        {
            var productId = inputModel.ProductId;

            var commentId = inputModel.CommentId;

            //Sanitize pageFragment
            pageFragment = this.javaScriptEncoder.Encode(pageFragment);

            //Store input model for passing in get action
            TempData["InputModelFromPOSTRequest"] = JsonSerializer.Serialize(inputModel);
            TempData["InputModelFromPOSTRequestType"] = nameof(EditReviewInputModel);

            //Check if data is valid without looking into the database
            if (this.ModelState.IsValid == false)
            {
                //Add suitable model state error for UI validation
                var newModelState = new ModelStateDictionary(this.ModelState);
                foreach (var modelStateEntry in this.ModelState.Values)
                {
                    foreach (var modelStateError in modelStateEntry.Errors)
                    {
                        newModelState.AddModelError($"CommentId_{inputModel.CommentCounter}", modelStateError.ErrorMessage);
                    }
                }

                //Store needed info for get request in TempData
                TempData["ErrorsFromPOSTRequest"] = ModelStateHelper.SerialiseModelState(newModelState);

                return this.RedirectToAction(nameof(ProductPage), "Products", new { productId = productId }, pageFragment);
            }

            var userId = this.userService.GetUserId(this.User.Identity.Name);

            var oldProductRating = new ProductRating();
            if(inputModel?.Rating > 0)
            {
                //Check if rating from this user for this product already exists
                if (this.productService.ProductRatingExists(userId, productId) == true)
                {
                    oldProductRating = this.productService.GetProductRating(userId, productId);
                }
                else
                {
                    this.ModelState.AddModelError($"Rating_{inputModel.CommentCounter}", "You have not given a review for this product!!!");
                }
            }
            else
            {
                oldProductRating = null;
            }


            var oldComment = new ProductComment();
            //Check if comment from this user for this product exists
            if (this.productCommentService.CommentExists(commentId) == true)
            {
                oldComment = this.productCommentService.GetProductComment(commentId);
            }
            else
            {
                oldComment = null;
                this.ModelState.AddModelError($"CommentId_{inputModel.CommentCounter}", "You have not given a review with a comment for this product!!!");
            }

            //Check if the comment belongs to this user and is for this product TODO
            if (this.productCommentService.CommentMatchesUserAndProduct(commentId, userId, productId) == false)
            {
                this.ModelState.AddModelError($"CommentId_{inputModel.CommentCounter}", "You have not given a review with a comment for this product!!!");
            }

            //Check if model state is valid after checking into the database
            if (this.ModelState.IsValid == false)
            {
                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData["ErrorsFromPOSTRequest"] = ModelStateHelper.SerialiseModelState(this.ModelState);

                //Reload same page with the TempData
                return this.RedirectToAction(nameof(ProductPage), "Products", new { productId = productId }, pageFragment);
            }

            if(oldComment != null) await this.productCommentService.EditCommentText(oldComment, inputModel.Text);
            if(oldProductRating != null) await this.productService.EditProductRating(oldProductRating, (double)inputModel.Rating);

            return this.RedirectToAction(nameof(ProductPage), "Products", new { productId = productId }, pageFragment);
        }
    }
}