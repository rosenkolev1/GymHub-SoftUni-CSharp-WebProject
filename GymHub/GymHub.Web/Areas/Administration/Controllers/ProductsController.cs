using AutoMapper;
using Azure.Storage.Blobs;
using GymHub.Common;
using GymHub.Data.Models;
using GymHub.Services;
using GymHub.Services.Messaging;
using GymHub.Services.ServicesFolder.AzureBlobService;
using GymHub.Services.ServicesFolder.CategoryService;
using GymHub.Services.ServicesFolder.ProductCommentService;
using GymHub.Services.ServicesFolder.ProductImageService;
using GymHub.Services.ServicesFolder.ProductService;
using GymHub.Web.AuthorizationPolicies;
using GymHub.Web.Helpers.NotificationHelpers;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymHub.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = GlobalConstants.AdminRoleName)]
    [Area("Administration")]
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
        private readonly ICategoryService categoryService;
        private readonly IProductImageService productImageService;
        private readonly IAzureBlobService azureBlobService;
        private readonly BlobServiceClient blobServiceClient;

        public ProductsController
            (IProductService productService, IMapper mapper, IProductCommentService productCommentService, IUserService userService,
            JavaScriptEncoder javaScriptEncoder, UserManager<User> userManager, SendGridEmailSender sendGridEmailSender,
            HtmlEncoder htmlEncoder, ICategoryService categoryService, IProductImageService productImageService,
            BlobServiceClient blobServiceClient, IAzureBlobService azureBlobService)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.productCommentService = productCommentService;
            this.userService = userService;
            this.javaScriptEncoder = javaScriptEncoder;
            this.userManager = userManager;
            this.sendGridEmailSender = sendGridEmailSender;
            this.htmlEncoder = htmlEncoder;
            this.categoryService = categoryService;
            this.productImageService = productImageService;
            this.blobServiceClient = blobServiceClient;
            this.azureBlobService = azureBlobService;
        }

        public IActionResult Add()
        {
            AddProductInputModel inputModel = null;

            //Add each model state error from the last action to this one
            if (TempData[GlobalConstants.ErrorsFromPOSTRequest] != null)
            {
                ModelStateHelper.MergeModelStates(TempData, ModelState);

                var inputModelJSON = TempData[GlobalConstants.InputModelFromPOSTRequest]?.ToString();
                inputModel = JsonSerializer.Deserialize<AddProductInputModel>(inputModelJSON);

                //Add categories names
                inputModel.CategoriesNames = new List<string>();
                var categoriesNames = this.categoryService.GetCategoryNamesFromIds(inputModel.CategoriesIds);
                inputModel.CategoriesNames.AddRange(categoriesNames);
            }

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductInputModel inputModel)
        {
            var newProduct = this.mapper.Map<Product>(inputModel);

            //Set input model short description
            inputModel.ShortDescription = this.productService.GetShordDescription(inputModel.Description, 40);

            //Store input model for passing in get action
            TempData[GlobalConstants.InputModelFromPOSTRequest] = JsonSerializer.Serialize(inputModel);
            TempData[GlobalConstants.InputModelFromPOSTRequestType] = nameof(AddProductInputModel);

            //Check without looking into the database
            if (this.ModelState.IsValid == false)
            {
                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

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

            //Check if there is a main image, regardless of the imageMode
            if ((inputModel.MainImage == null && inputModel.ImagesAsFileUploads == false) || (inputModel.MainImageUpload == null && inputModel.ImagesAsFileUploads == true))
            {
                this.ModelState.AddModelError("", "Main image is required");

                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                return this.RedirectToAction(nameof(Add), "Products");
            }

            //CHECK THE IMAGES LINKS
            if (inputModel.ImagesAsFileUploads == false)
            {
                //Check if all of these images are unique
                if (this.productImageService.ImagesAreRepeated(inputModel.MainImage, inputModel.AdditionalImages))
                {
                    this.ModelState.AddModelError("", "There are 2 or more non-unique images");
                }

                //Check if main image is already used
                if (this.productImageService.ProductImageExists(inputModel.MainImage) == true)
                {
                    this.ModelState.AddModelError("MainImage", "This image is already used.");
                }

                if (inputModel.AdditionalImages == null) inputModel.AdditionalImages = new List<string>();

                //Check if any of the additional images are used
                for (int i = 0; i < inputModel.AdditionalImages.Count; i++)
                {
                    var additionalImage = inputModel.AdditionalImages[i];
                    if (this.productImageService.ProductImageExists(additionalImage) == true)
                    {
                        this.ModelState.AddModelError($"AdditionalImages[{i}]", "This image is already used.");
                    }
                }
            }
            //CHECK IMAGES UPLOADS
            else
            {
                //Check main image upload
                if (this.productImageService.ValidImageExtension(inputModel.MainImageUpload) == false)
                {
                    this.ModelState.AddModelError("MainImageUpload", "The uploaded image is invalid");
                }

                if (inputModel.AdditionalImagesUploads == null) inputModel.AdditionalImagesUploads = new List<IFormFile>();

                //Check additional images uploads
                for (int i = 0; i < inputModel.AdditionalImagesUploads.Count; i++)
                {
                    var imageUpload = inputModel.AdditionalImagesUploads[i];
                    if (this.productImageService.ValidImageExtension(imageUpload) == false)
                    {
                        this.ModelState.AddModelError($"AdditionalImageUpload[{i}]", "The uploaded image is invalid");
                    }
                }
            }

            //Check if categories exist in the database or if there are even any categories for this product
            for (int i = 0; i < inputModel.CategoriesIds.Count; i++)
            {
                var categoryId = inputModel.CategoriesIds[i];
                if (this.categoryService.CategoryExists(categoryId) == false)
                {
                    this.ModelState.AddModelError($"CategoriesIds{i}", "This category doesn't exist");
                }
            }
            if (inputModel.CategoriesIds.Count == 0)
            {
                this.ModelState.AddModelError("", "You need to add at least one category for this product");
            }

            //Check if categories are unique
            if (inputModel.CategoriesIds.Where(x => string.IsNullOrWhiteSpace(x) == false).Distinct().Count() != inputModel.CategoriesIds.Where(x => string.IsNullOrWhiteSpace(x) == false).Count())
            {
                this.ModelState.AddModelError($"", "One category cannot be used multiple times.");
            }

            if (this.ModelState.IsValid == false)
            {
                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                return this.RedirectToAction(nameof(Add), "Products");
            }

            //Add images to product
            if (inputModel.ImagesAsFileUploads == false)
            {
                newProduct.Images = new List<ProductImage>();

                //Set additional images
                newProduct.Images = inputModel.AdditionalImages
                    .Where(x => x != null)
                    .Select(x => new ProductImage { Image = x, Product = newProduct }).ToList();

                //Set main image
                newProduct.Images.Add(new ProductImage { Image = inputModel.MainImage, IsMain = true, Product = newProduct, ProductId = newProduct.Id });
            }
            else
            {
                newProduct.Images = new List<ProductImage>();

                //Upload main image
                var imageUrl = await this.productImageService.UploadImageAsync(inputModel.MainImageUpload, newProduct);

                //Set the main image
                newProduct.Images.Add(new ProductImage { Image = imageUrl, IsMain = true, Product = newProduct, ProductId = newProduct.Id });

                //Upload the additional images and set the additional images
                foreach (var additionalImage in inputModel.AdditionalImagesUploads)
                {
                    var additionalImageUrl = await this.productImageService.UploadImageAsync(additionalImage, newProduct);
                    newProduct.Images.Add(new ProductImage { Image = additionalImageUrl, IsMain = false, Product = newProduct, ProductId = newProduct.Id });
                }
            }

            //Add product
            await this.productService.AddAsync(newProduct);

            //Add categories
            await this.categoryService.AddCategoriesToProductAsync(newProduct, inputModel.CategoriesIds);

            //Set notification
            NotificationHelper.SetNotification(this.TempData, NotificationType.Success, "Product was successfully added");

            return this.RedirectToAction("ProductPage", "Products", new { area = "" , productId = newProduct.Id });
        }

        public IActionResult LoadCategoryInput(AddCategoryToProductViewModel viewModel)
        {
            viewModel.Categories = this.mapper.Map<List<CategoryViewModel>>(this.categoryService.GetAllCategories());

            return this.PartialView("/Areas/Administration/Views/Products/_AddCategoryToProductPartial.cshtml", viewModel);
        }

        public IActionResult Edit(string productId)
        {
            if (this.productService.ProductExistsById(productId) == false)
            {
                NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "The product doesn't exist");

                return this.RedirectToAction("ProductPage", "Products", new { area = "", productId = productId });
            }

            EditProductInputModel inputModel = null;

            //Add each model state error from the last action to this one. Fill the input model with he values from the last post action
            if (TempData[GlobalConstants.ErrorsFromPOSTRequest] != null && TempData[GlobalConstants.InputModelFromPOSTRequestType]?.ToString() == nameof(EditProductInputModel))
            {
                ModelStateHelper.MergeModelStates(TempData, this.ModelState);

                var inputModelJSON = TempData[GlobalConstants.InputModelFromPOSTRequest]?.ToString();
                inputModel = JsonSerializer.Deserialize<EditProductInputModel>(inputModelJSON);
                var product = this.productService.GetProductById(productId, true);

                //Add categories names
                inputModel.CategoriesNames = new List<string>();
                var categoriesNames = this.categoryService.GetCategoryNamesFromIds(inputModel.CategoriesIds);
                inputModel.CategoriesNames.AddRange(categoriesNames);

                //Set the image path for all of the modifiedImages
                if (inputModel.MainImageUploadInfo.ModifiedImage.Id != null)
                {
                    inputModel.MainImageUploadInfo.ModifiedImage.Path = this.productImageService.GetImageById(inputModel.MainImageUploadInfo.ModifiedImage.Id).Image;
                }

                //Set isBeingModified param for mainImageUpload to false
                inputModel.MainImageUploadInfo.IsBeingModified = false;

                foreach (var additionalImageUploadInfo in inputModel.AdditionalImagesUploadsInfo)
                {
                    var modifiedImage = additionalImageUploadInfo.ModifiedImage;
                    if (modifiedImage.Id != null) modifiedImage.Path = this.productImageService.GetImageById(modifiedImage.Id).Image;

                    //Set isBeingModified param for additionalImageUpload to false
                    additionalImageUploadInfo.IsBeingModified = false;
                }
            }
            //If there wasn't an error with the edit form prior to this, just fill the inputModel like normal
            else
            {
                var product = this.productService.GetProductById(productId, true);
                inputModel = this.mapper.Map<EditProductInputModel>(product);

                //Get the categories for the product
                var productCategories = this.categoryService.GetCategoriesForProduct(product.Id).ToList();
                inputModel.CategoriesIds = productCategories.Select(x => x.Id).ToList();
                inputModel.CategoriesNames = productCategories.Select(x => x.Name).ToList();

                //Set image mode
                inputModel.ImagesAsFileUploads = false;

                //Get the main image
                var mainImage = this.productImageService.GetMainImage(productId);

                //Set main image path
                inputModel.MainImage = mainImage.Image;

                //Set main image upload info
                inputModel.MainImageUploadInfo = new EditProductImageUploadInputModel
                {
                    ImageUpload = null,
                    ModifiedImage = new ImageIdAndPathInputModel { Id = mainImage.Id, Path = mainImage.Image }
                };

                //Get additional images
                var additionalImages = this.productImageService.GetAdditionalImages(productId);

                inputModel.AdditionalImages = new List<string>();
                inputModel.AdditionalImagesUploadsInfo = new List<EditProductImageUploadInputModel>();

                //Sed additional images uploads and paths
                for (int i = 0; i < additionalImages.Count; i++)
                {
                    var image = additionalImages[i];
                    inputModel.AdditionalImages.Add(image.Image);
                    inputModel.AdditionalImagesUploadsInfo.Add(new EditProductImageUploadInputModel
                    {
                        ImageUpload = null,
                        ModifiedImage = new ImageIdAndPathInputModel { Id = image.Id, Path = image.Image }
                    });
                }
            }

            //Set input model short description
            inputModel.ShortDescription = this.productService.GetShordDescription(inputModel.Description, 40);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductInputModel inputModel)
        {
            //Set input model short description
            inputModel.ShortDescription = this.productService.GetShordDescription(inputModel.Description, 40);

            //Store input model for passing in get action
            TempData[GlobalConstants.InputModelFromPOSTRequest] = JsonSerializer.Serialize(inputModel);
            TempData[GlobalConstants.InputModelFromPOSTRequestType] = nameof(EditProductInputModel);

            //Check without looking into the database
            if (this.ModelState.IsValid == false)
            {
                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

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
            if (this.productService.ProductExistsByName(inputModel.Name, productId))
            {
                this.ModelState.AddModelError("Name", "Product with this name already exists.");
            }

            //Check if there is a main image, regardless of the imageMode
            if ((inputModel.MainImage == null && inputModel.ImagesAsFileUploads == false) || (inputModel.MainImageUploadInfo.ImageUpload == null && inputModel.MainImageUploadInfo.IsBeingModified && inputModel.ImagesAsFileUploads == true))
            {
                this.ModelState.AddModelError("", "Main image is required");

                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                return this.RedirectToAction(nameof(Edit), "Products", new { productId = productId });
            }

            //CHECK THE IMAGES LINKS
            if (inputModel.ImagesAsFileUploads == false)
            {
                //Check if all of these images are unique
                if (this.productImageService.ImagesAreRepeated(inputModel.MainImage, inputModel.AdditionalImages))
                {
                    this.ModelState.AddModelError("", "There are 2 or more non-unique images");
                }

                //Check if main image is already used by OTHER products
                if (this.productImageService.ProductImageExists(inputModel.MainImage, productId) == true)
                {
                    this.ModelState.AddModelError("MainImage", "This image is already used.");
                }

                if (inputModel.AdditionalImages == null) inputModel.AdditionalImages = new List<string>();

                //Check if any of the additional images are used by OTHER products
                for (int i = 0; i < inputModel.AdditionalImages.Count; i++)
                {
                    var additionalImage = inputModel.AdditionalImages[i];
                    if (this.productImageService.ProductImageExists(additionalImage, productId) == true)
                    {
                        this.ModelState.AddModelError($"AdditionalImages[{i}]", "This image is already used.");
                    }
                }
            }
            //CHECK IMAGES UPLOADS
            else
            {
                var mainImageUpload = inputModel.MainImageUploadInfo.ImageUpload;

                //Check main image upload extension is valid, but only if the user has actually selected a file themselves
                if (inputModel.MainImageUploadInfo.IsBeingModified && this.productImageService.ValidImageExtension(mainImageUpload) == false)
                {
                    this.ModelState.AddModelError("MainImageUploadInfo.ImageUpload", "The uploaded image is invalid");
                }

                if (inputModel.AdditionalImagesUploadsInfo == null) inputModel.AdditionalImagesUploadsInfo = new List<EditProductImageUploadInputModel>();

                var additionalImagesUploads = inputModel.AdditionalImagesUploadsInfo
                    .Select(x => x.ImageUpload)
                    .Where(imageUpload => imageUpload != null)
                    .ToList();

                //Check additional images uploads
                for (int i = 0; i < additionalImagesUploads.Count; i++)
                {
                    var imageUpload = additionalImagesUploads[i];
                    if (imageUpload != null && this.productImageService.ValidImageExtension(imageUpload) == false)
                    {
                        this.ModelState.AddModelError($"AdditionalImagesUploadsInfo[{i}].ImageUpload.", "The uploaded image is invalid");
                    }
                }
            }

            //Check if categories exist in the database or if there are even any categories for this product
            for (int i = 0; i < inputModel.CategoriesIds.Count; i++)
            {
                var categoryId = inputModel.CategoriesIds[i];
                if (this.categoryService.CategoryExists(categoryId) == false)
                {
                    this.ModelState.AddModelError($"CategoriesIds{i}", "This category doesn't exist");
                }
            }
            if (inputModel.CategoriesIds.Count == 0)
            {
                this.ModelState.AddModelError("", "You need to add at least one category for this product");
            }

            //Check if categories exist in the database
            for (int i = 0; i < inputModel.CategoriesIds.Count; i++)
            {
                var categoryId = inputModel.CategoriesIds[i];
                if (this.categoryService.CategoryExists(categoryId) == false)
                {
                    this.ModelState.AddModelError($"CategoriesIds{i}", "This category doesn't exist");
                }
            }

            //Check if categories are unique
            if (inputModel.CategoriesIds.Where(x => string.IsNullOrWhiteSpace(x) == false).Distinct().Count() != inputModel.CategoriesIds.Where(x => string.IsNullOrWhiteSpace(x) == false).Count())
            {
                this.ModelState.AddModelError($"", "One category cannot be used multiple times.");
            }


            if (this.ModelState.IsValid == false)
            {
                //Store needed info for get request in TempData only if the model state is invalid after doing the complex checks
                TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                return this.RedirectToAction(nameof(Edit), "Products", new { productId = inputModel.Id });
            }

            await this.productService.EditAsync(inputModel);

            var product = this.productService.GetProductById(inputModel.Id, false);

            await this.categoryService.EditCategoriesToProductAsync(product, inputModel.CategoriesIds);

            //Set notification
            NotificationHelper.SetNotification(this.TempData, NotificationType.Success, "Product was successfully edited");

            return this.RedirectToAction("ProductPage", "Products", new { area = "", productId = inputModel.Id });
        }

        [IgnoreAntiforgeryToken]
        public IActionResult LoadAddProductPreview(string inputModelJSON)
        {
            var inputModel = JsonSerializer.Deserialize<EditProductInputModel>(inputModelJSON);

            //Set input model short description
            inputModel.ShortDescription = this.productService.GetShordDescription(inputModel.Description, 40);

            return this.PartialView("/Areas/Administration/Views/Products/_AddProductPreviewPartial.cshtml", inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string productId, string errorReturnUrl)
        {
            if (this.productService.ProductExistsById(productId) == false)
            {
                this.ModelState.AddModelError("removeProduct_productId", "This product doesn't exist");
            }

            if (this.ModelState.IsValid == false)
            {
                //Add notification
                NotificationHelper.SetNotification(TempData, NotificationType.Error, "An error occured while removing product. Product wasn't removed");

                //Store needed info for get request in TempData
                TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                return this.Redirect(errorReturnUrl);
            }

            await this.productService.RemoveProductAsync(productId);

            NotificationHelper.SetNotification(TempData, NotificationType.Success, "Successfully remove product.");

            return this.RedirectToAction("All", "Products", new { area = ""});
        }
    }
}
