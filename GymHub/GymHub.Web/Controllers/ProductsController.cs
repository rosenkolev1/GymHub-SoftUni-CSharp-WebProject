using AutoMapper;
using GymHub.Common;
using GymHub.Data.Models;
using GymHub.Services;
using GymHub.Web.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> userManager;
        public ProductsController(IProductService productService, IMapper mapper, IProductCommentService productCommentService, IUserService userService, JavaScriptEncoder javaScriptEncoder, UserManager<User> userManager)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.productCommentService = productCommentService;
            this.userService = userService;
            this.javaScriptEncoder = javaScriptEncoder;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> All()
        {
            var products = this.productService.GetAllProducts();
            foreach (var product in products)
            {
                product.ShortDescription = this.productService.GetShordDescription(product.Description, 40);
            }

            //This is for debugging purposes for now
            this.TempData.Clear();

            return this.View(products);
        }

        [Authorize]
        public async Task<IActionResult> ProductPage(string productId, string toReplyComment)
        {
            var product = this.productService.GetProductById(productId);
            var viewModel = mapper.Map<ProductInfoViewModel>(product);

            ////Load commentsLikes for each comment
            //foreach (var productComment in viewModel.ProductComments)
            //{
            //    await this.productCommentService.LoadCommentLikes(productComment);
            //}

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

            //toReplyComment query string
            viewModel.ToReplyComment = toReplyComment;

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

            //If input model is for adding review
            if (typeOfInputModel?.ToString() == nameof(AddReviewInputModel))
            {
                complexModel = AssignViewAndInputModels<AddReviewInputModel, ProductInfoViewModel>(viewModel);
            }
            //If input model is for replying to a comment
            else if (typeOfInputModel?.ToString() == nameof(ReplyCommentInputModel))
            {
                var replyCommentInputModelsJSON = TempData["InputModelFromPOSTRequest"]?.ToString();
                var replyCommentInputModel = JsonSerializer.Deserialize<ReplyCommentInputModel>(replyCommentInputModelsJSON);
                viewModel.ReplyCommentInputModel = replyCommentInputModel;

                complexModel = AssignViewAndInputModels<AddReviewInputModel, ProductInfoViewModel>(viewModel, true);
            }
            //If there isn't an input model
            else
            {
                complexModel = AssignViewAndInputModels<AddReviewInputModel, ProductInfoViewModel>(viewModel, true);
            }

            return this.View("ProductPage", complexModel);
        }

        [Authorize]
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

            //Sanitize pageFragment
            pageFragment = this.javaScriptEncoder.Encode(pageFragment);

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
            if (this.productService.ProductRatingExists(newProductRating) == true && this.productCommentService.CommentExists(newComment) == true)
            {
                this.ModelState.AddModelError("InputModel.Rating", "You have already given a review for this product!!!");
            }

            //Check if rating from this user for this product already exists
            else if (this.productService.ProductRatingExists(newProductRating) == true)
            {
                this.ModelState.AddModelError("InputModel.Rating", "You have already given a review with this rating for this product!!!");
            }

            //Check if comment already from this user for this product already exists(as part of his review, he can still have comments as replies to other people for the same product)
            else if (this.productCommentService.CommentExists(newComment) == true)
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

            //Set the rating foreign key for new comment
            var newRating = new ProductRating
            {
                ProductCommentId = newComment.Id,
                ProductId = productId,
                UserId = userId,
                Rating = (int)complexModel.InputModel.Rating
            };

            await this.productCommentService.AddAsync(newComment);

            newComment.ProductRating = newRating;

            await this.productService.AddRatingAsync(newRating);

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
            if (inputModel?.ProductRatingViewModel?.AverageRating > 0)
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

            if (oldComment != null) await this.productCommentService.EditCommentTextAsync(oldComment, inputModel.Text);
            if (oldProductRating != null) await this.productService.EditProductRating(oldProductRating, (double)inputModel.ProductRatingViewModel.AverageRating);

            return this.RedirectToAction(nameof(ProductPage), "Products", new { productId = productId, toReplyComment = oldComment.Id }, pageFragment);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ReplyToComment(ReplyCommentInputModel inputModel, string pageFragment)
        {
            var productId = inputModel.ProductId;

            var commentId = inputModel.ParentCommentId;

            //Sanitize pageFragment
            pageFragment = this.javaScriptEncoder.Encode(pageFragment);

            //Check if data is valid without looking into the database
            if (this.ModelState.IsValid == false)
            {
                //Store input model for passing in get action
                TempData["InputModelFromPOSTRequest"] = JsonSerializer.Serialize(inputModel);
                TempData["InputModelFromPOSTRequestType"] = nameof(ReplyCommentInputModel);

                //Add suitable model state error for UI validation
                var newModelState = new ModelStateDictionary(this.ModelState);
                foreach (var modelStateEntry in this.ModelState.Values)
                {
                    foreach (var modelStateError in modelStateEntry.Errors)
                    {
                        newModelState.AddModelError($"ParentCommentId_{inputModel.CommentCounter}", modelStateError.ErrorMessage);
                    }
                }

                //Store needed info for get request in TempData
                TempData["ErrorsFromPOSTRequest"] = ModelStateHelper.SerialiseModelState(newModelState);

                return this.RedirectToAction(nameof(ProductPage), "Products", new { productId = productId }, pageFragment);
            }

            var userId = this.userService.GetUserId(this.User.Identity.Name);

            //Check if parent comment for this product exists
            if (this.productCommentService.CommentExists(commentId) == false)
            {
                this.ModelState.AddModelError($"ParentCommentId_{inputModel.CommentCounter}", "Can't reply to nonexistent comment.");
            }

            //Check if the user isn't trying to reply to himself
            if (this.productCommentService.CommentBelongsToUser(commentId, userId))
            {
                this.ModelState.AddModelError($"ParentCommentId_{inputModel.CommentCounter}", "Can't reply to yourself.");
            }

            //Create new reply comment
            var replyComment = new ProductComment
            {
                ProductId = productId,
                ParentCommentId = commentId,
                Text = inputModel.Text,
                CommentedOn = DateTime.UtcNow,
                UserId = userId
            };

            //Check if model state is valid after checking into the database
            if (this.ModelState.IsValid == false)
            {
                //Store input model for passing in get action
                TempData["InputModelFromPOSTRequest"] = JsonSerializer.Serialize(inputModel);
                TempData["InputModelFromPOSTRequestType"] = nameof(ReplyCommentInputModel);

                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData["ErrorsFromPOSTRequest"] = ModelStateHelper.SerialiseModelState(this.ModelState);

                //Reload same page with the TempData
                return this.RedirectToAction(nameof(ProductPage), "Products", new { productId = productId }, pageFragment);
            }

            await this.productCommentService.AddAsync(replyComment);

            return this.RedirectToAction(nameof(ProductPage), "Products", new { productId = productId, toReplyComment = replyComment.Id }, pageFragment);
        }

        [Authorize]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> LoadReplyToComment(ReplyCommentInputModel inputModel)
        {
            return this.PartialView("Views/Products/_ProductCommentReplyPartial.cshtml", inputModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveComment(RemoveCommentInputModel removeCommentInputModel, string pageFragment, string productId)
        {
            var commentId = removeCommentInputModel.RemoveCommentId;

            //Sanitize pageFragment
            pageFragment = this.javaScriptEncoder.Encode(pageFragment);

            if (this.ModelState.IsValid == false)
            {
                return this.RedirectToAction(nameof(All), "Products");
            }

            var userId = this.userService.GetUserId(this.User.Identity.Name);

            //Check if comment belongs to user and/or exists
            if (this.productCommentService.CommentMatchesUserAndProduct(commentId, userId, productId) == false && this.User.IsInRole(GlobalConstants.AdminRoleName) == false)
            {
                this.ModelState.AddModelError(nameof(removeCommentInputModel.RemoveCommentId), "Comment doesn't exist");
            }

            if (this.ModelState.IsValid == false)
            {
                //Store input model for passing in get action
                TempData["InputModelFromPOSTRequest"] = JsonSerializer.Serialize(removeCommentInputModel);
                TempData["InputModelFromPOSTRequestType"] = nameof(ReplyCommentInputModel);
                return this.RedirectToAction(nameof(All), "Products");
            }

            await this.productCommentService.RemoveAsync(commentId);

            return this.RedirectToAction(nameof(ProductPage), "Products", new { productId = productId }, pageFragment);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LikeComment(string commentId)
        {
            var userId = this.userService.GetUserId(this.User.Identity.Name);

            if(this.productCommentService.UserHasLikedComment(commentId, userId) == true)
            {
                this.productCommentService.UnlikeCommentAsync(commentId, userId);
            }
            else
            {
                await this.productCommentService.LikeCommentAsync(commentId, userId);
            }

            return this.Ok();
        }
    }
}