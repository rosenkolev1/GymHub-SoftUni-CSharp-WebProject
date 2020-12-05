﻿using AutoMapper;
using GymHub.Common;
using GymHub.Data.Models;
using GymHub.Data.Models.Enums;
using GymHub.Services;
using GymHub.Services.Messaging;
using GymHub.Web.AuthorizationPolicies;
using GymHub.Web.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        private readonly HtmlEncoder htmlEncoder;
        private readonly UserManager<User> userManager;
        private readonly SendGridEmailSender sendGridEmailSender;
        public ProductsController
            (IProductService productService, IMapper mapper, IProductCommentService productCommentService, IUserService userService,
            JavaScriptEncoder javaScriptEncoder, UserManager<User> userManager, SendGridEmailSender sendGridEmailSender,
            HtmlEncoder htmlEncoder)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.productCommentService = productCommentService;
            this.userService = userService;
            this.javaScriptEncoder = javaScriptEncoder;
            this.userManager = userManager;
            this.sendGridEmailSender = sendGridEmailSender;
            this.htmlEncoder = htmlEncoder;
        }

        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        public async Task<IActionResult> Add()
        {
            AddProductInputModel inputModel = null;

            //Add each model state error from the last action to this one
            if (TempData["ErrorsFromPOSTRequest"] != null)
            {
                var postRequestModelState = ModelStateHelper.DeserialiseModelState(TempData["ErrorsFromPOSTRequest"].ToString());
                this.ModelState.Merge(postRequestModelState);

                var inputModelJSON = TempData["InputModelFromPOSTRequest"]?.ToString();
                inputModel = JsonSerializer.Deserialize<AddProductInputModel>(inputModelJSON);
            }

            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        public async Task<IActionResult> Add(AddProductInputModel inputModel)
        {
            var newProduct = this.mapper.Map<Product>(inputModel);

            //Set additional images
            newProduct.AdditionalImages = inputModel.AdditionalImages
                .Where(x => x != null)
                .Select(x => new ProductImage { Image = x, Product = newProduct }).ToList();

            //Set input model short description
            inputModel.ShortDescription = this.productService.GetShordDescription(inputModel.Description, 40);

            //Store input model for passing in get action
            TempData["InputModelFromPOSTRequest"] = JsonSerializer.Serialize(inputModel);
            TempData["InputModelFromPOSTRequestType"] = nameof(AddProductInputModel);

            //Check without looking into the database
            if (this.ModelState.IsValid == false)
            {
                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData["ErrorsFromPOSTRequest"] = ModelStateHelper.SerialiseModelState(this.ModelState);

                return this.RedirectToAction(nameof(Add), "Products");
            }

            //Check if product with this model or name
            if (this.productService.ProductExistsByModel(inputModel.Model) == true)
            {
                this.ModelState.AddModelError("Model", "Product with this model already exists.");
            }
            if (this.productService.ProductExistsByName(inputModel.Name))
            {
                this.ModelState.AddModelError("Name", "Product with this name already exists.");
            }

            //Check if all of these images are unique
            if (this.productService.ImagesAreRepeated(inputModel.MainImage, inputModel.AdditionalImages))
            {
                this.ModelState.AddModelError("", "There are 2 or more non-unique images");
            }

            //Check if main image is already used
            if (this.productService.ProductImageExists(inputModel.MainImage) == true)
            {
                this.ModelState.AddModelError("MainImage", "This image is already used.");
            }

            //Check if any of the additional images are used
            for (int i = 0; i < inputModel.AdditionalImages.Count; i++)
            {
                var additionalImage = inputModel.AdditionalImages[i];
                if (this.productService.ProductImageExists(additionalImage) == true)
                {
                    this.ModelState.AddModelError($"AdditionalImages[{i}]", "This image is already used.");
                }
            }

            if (this.ModelState.IsValid == false)
            {
                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData["ErrorsFromPOSTRequest"] = ModelStateHelper.SerialiseModelState(this.ModelState);

                return this.RedirectToAction(nameof(Add), "Products");
            }

            await this.productService.AddAsync(newProduct);

            return this.RedirectToAction(nameof(ProductPage), "Products", new { productId = newProduct.Id });
        }

        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        public async Task<IActionResult> Edit(string productId, string errorReturnUrl)
        {
            //TODO add the errorReturnURL
            //Check if product with this id exists. MAYBE I WILL REPLACE THIS LATER WILL HAVE TO SWITCH TO DEVELOPMENT AND SEED
            if (this.productService.ProductExistsById(productId) == false)
            {
                return this.View("/Views/Shared/Error.cshtml");
            }

            AddProductInputModel inputModel = null;

            //Add each model state error from the last action to this one. Fill the input model with he values from the last post action
            if (TempData["ErrorsFromPOSTRequest"] != null && TempData["InputModelFromPOSTRequestType"]?.ToString() == nameof(AddProductInputModel))
            {
                var postRequestModelState = ModelStateHelper.DeserialiseModelState(TempData["ErrorsFromPOSTRequest"].ToString());
                this.ModelState.Merge(postRequestModelState);

                var inputModelJSON = TempData["InputModelFromPOSTRequest"]?.ToString();
                inputModel = JsonSerializer.Deserialize<AddProductInputModel>(inputModelJSON);

            }
            //If there wasn't an error with the edit form prior to this, just fill the inputModel like normal
            else
            {
                var product = this.productService.GetProductById(productId);
                inputModel = this.mapper.Map<AddProductInputModel>(product);

                //Set the correct additional images paths
                for (int i = 0; i < product.AdditionalImages.Count; i++)
                {
                    var image = product.AdditionalImages.ToList()[i];
                    inputModel.AdditionalImages[i] = image.Image;
                }
            }

            //Set the input model mode to edit
            inputModel.IsAdding = false;

            return this.View("/Views/Products/Add.cshtml", inputModel);
        }

        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        [HttpPost]
        public async Task<IActionResult> Edit(AddProductInputModel inputModel)
        {
            //Set input model short description
            inputModel.ShortDescription = this.productService.GetShordDescription(inputModel.Description, 40);

            //Store input model for passing in get action
            TempData["InputModelFromPOSTRequest"] = JsonSerializer.Serialize(inputModel);
            TempData["InputModelFromPOSTRequestType"] = nameof(AddProductInputModel);

            //Set input model mode to edit
            inputModel.IsAdding = false;

            //Check without looking into the database
            if (this.ModelState.IsValid == false)
            {
                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData["ErrorsFromPOSTRequest"] = ModelStateHelper.SerialiseModelState(this.ModelState);

                return this.RedirectToAction(nameof(Edit), "Products", new { productId = inputModel.Id });
            }

            //Check if product with this id exists
            if (this.productService.ProductExistsById(inputModel.Id) == false)
            {
                this.ModelState.AddModelError("", "The product that you are trying to edit doesn't exist.");
            }

            var productId = inputModel.Id;

            //Check if product with this model or name exist and it is not the product that is currently being edited
            if (this.productService.ProductExistsByModel(inputModel.Model, productId) == true)
            {
                this.ModelState.AddModelError("Model", "Product with this model already exists.");
            }
            if(this.productService.ProductExistsByName(inputModel.Name, productId))
            {
                this.ModelState.AddModelError("Name", "Product with this name already exists.");
            }

            //Check if all of these images are unique
            if(this.productService.ImagesAreRepeated(inputModel.MainImage, inputModel.AdditionalImages))
            {
                this.ModelState.AddModelError("", "There are 2 or more non-unique images");
            }

            //Check if main image is already used
            if (this.productService.ProductImageExists(inputModel.MainImage, productId) == true)
            {
                this.ModelState.AddModelError("MainImage", "This image is already used.");
            }

            //Check if any of the additional images are used
            for (int i = 0; i < inputModel.AdditionalImages.Count; i++)
            {
                var additionalImage = inputModel.AdditionalImages[i];
                if (this.productService.ProductImageExists(additionalImage, productId) == true)
                {
                    this.ModelState.AddModelError($"AdditionalImages[{i}]", "This image is already used.");
                }
            }

            if (this.ModelState.IsValid == false)
            {
                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData["ErrorsFromPOSTRequest"] = ModelStateHelper.SerialiseModelState(this.ModelState);

                return this.RedirectToAction(nameof(Edit), "Products", new { productId = inputModel.Id});
            }

            await this.productService.EditAsync(inputModel);

            return this.RedirectToAction(nameof(ProductPage), "Products", new { productId = inputModel.Id});
        }

        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> LoadAddProductPreview(string inputModelJSON)
        {
            var inputModel = JsonSerializer.Deserialize<AddProductInputModel>(inputModelJSON);

            //Set input model short description
            inputModel.ShortDescription = this.productService.GetShordDescription(inputModel.Description, 40);

            return this.PartialView("Views/Products/_AddProductPreviewPartial.cshtml", inputModel);
        }

        [HttpPost]
        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        public async Task<IActionResult> Remove(string productId, string errorReturnUrl)
        {
            if (this.productService.ProductExistsById(productId) == false)
            {
                this.ModelState.AddModelError("removeProduct_productId", "This product doesn't exist");
            }

            if (this.ModelState.IsValid == false)
            {
                //TODO: add notification for failed removal of product

                //Store needed info for get request in TempData
                TempData["ErrorsFromPOSTRequest"] = ModelStateHelper.SerialiseModelState(this.ModelState);

                return this.Redirect(errorReturnUrl);
            }

            //TODO: Add notification for successful removal of product 
            await this.productService.RemoveProductAsync(productId);

            return this.RedirectToAction(nameof(All));
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
        public async Task<IActionResult> ProductPage(string productId, string toReplyComment, int commentsPage, int commentsOrderingOption)
        {
            var product = this.productService.GetProductById(productId);
            var viewModel = mapper.Map<ProductInfoViewModel>(product);

            //Sanitize commentsPage
            commentsPage = int.Parse(this.htmlEncoder.Encode(commentsPage.ToString()));

            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);
            viewModel.CurrentUserId = currentUserId;

            //Short description
            viewModel.ShortDescription = this.productService.GetShordDescription(viewModel.Description, 40);

            //Overall product rating
            viewModel.ProductRating = new ProductRatingViewModel(this.productService.GetAverageRating(viewModel.ProductRatings));

            await FillProductInfoViewModel(viewModel, productId, commentsPage, toReplyComment, commentsOrderingOption);

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
        private ComplexModel<InputModelType, ViewModelType> AssignViewAndInputModels<InputModelType, ViewModelType>(ViewModelType viewModel, bool onlyViewModel = false)
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

        private async Task FillProductInfoViewModel(ProductInfoViewModel viewModel, string productId, int commentsPage, string toReplyComment, int commentsOrderingOption)
        {
            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);
            viewModel.CurrentUserId = currentUserId;

            //Product comments
            foreach (var comment in viewModel.ProductComments)
            {
                if (comment.ParentCommentId == null)
                {
                    var childComments = await this.productCommentService.GetAllChildCommentsAsync(comment);

                    viewModel.ParentsChildrenComments.Add(comment, childComments);
                }
            }

            commentsPage = commentsPage <= 0 || (commentsPage - 1) * GlobalConstants.CommentsPerPage >= viewModel.ParentsChildrenComments.Keys.Count ? 1 : commentsPage;
            viewModel.NumberOfCommentsPages = (viewModel.ParentsChildrenComments.Keys.Count % GlobalConstants.CommentsPerPage == 0) 
                ? (viewModel.ParentsChildrenComments.Keys.Count / GlobalConstants.CommentsPerPage) 
                : (viewModel.ParentsChildrenComments.Keys.Count / GlobalConstants.CommentsPerPage) + 1;

            viewModel.CurrentCommentsPage = commentsPage;

            //Select the parent comments from the comments page and order these comments
            if (commentsOrderingOption < 1 || commentsOrderingOption > typeof(ProductCommentsOrderingOptions).GetEnumValues().Length) commentsOrderingOption = 1;
            viewModel.CommentsOrderingOptions = (ProductCommentsOrderingOptions)commentsOrderingOption;

            viewModel.ParentsChildrenComments = this.productCommentService.OrderParentsChildrenComments(viewModel.ParentsChildrenComments
                     .OrderByDescending(kv => kv.Key.UserId == currentUserId), (ProductCommentsOrderingOptions)commentsOrderingOption)
                     .Skip((commentsPage - 1) * GlobalConstants.CommentsPerPage)
                     .Take(GlobalConstants.CommentsPerPage)
                     .ToDictionary(x => x.Key, x => x.Value);

            //Order the comments' replies
            for (int i = 0; i < viewModel.ParentsChildrenComments.Count; i++)
            {
                var kv = viewModel.ParentsChildrenComments.ElementAt(i);
                viewModel.ParentsChildrenComments[kv.Key] = viewModel.ParentsChildrenComments[kv.Key]
                    .OrderBy(x => x.CommentedOn)
                    .ToList();
            }

            //Select the product ratings for the selected comments
            viewModel.ProductRatings = viewModel.ProductRatings
                .Where(x => viewModel.ParentsChildrenComments.Keys.Any(y => y.Id == x.ProductCommentId))
                .ToList();

            //Product rating for each user
            foreach (var userProductRating in viewModel.ProductRatings)
            {
                viewModel.UsersProductRatings.Add(userProductRating.User, new ProductRatingViewModel(userProductRating.Rating));
            }

            //toReplyComment query string
            viewModel.ToReplyComment = toReplyComment;

            //Check if user has already given a review on this product
            viewModel.ReviewedByCurrentUser = this.productService.ProductRatingExists(currentUserId, productId);
        }
    }
}