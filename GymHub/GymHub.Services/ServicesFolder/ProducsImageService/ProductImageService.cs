using GymHub.Common;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using GymHub.Services.ServicesFolder.AzureBlobService;
using GymHub.Web.Models.InputModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.ProductImageService
{
    public class ProductImageService : DeleteableEntityService, IProductImageService
    {
        private readonly IAzureBlobService azureBlobService;

        public ProductImageService(ApplicationDbContext context, IAzureBlobService azureBlobService)
            :base(context)
        {
            this.azureBlobService = azureBlobService;
        }

        public async Task AddProductImageAsync(ProductImage image)
        {
            await context.ProductsImages.AddAsync(image);
            await context.SaveChangesAsync();
        }

        public List<ProductImage> GetAdditionalImages(string productId)
        {
            return this.context.ProductsImages
                .Where(x => x.ProductId == productId & x.IsMain == false).ToList();
        }

        public ProductImage GetImageByPath(string imagePath)
        {
            return this.context.ProductsImages.FirstOrDefault(x => x.Image == imagePath);
        }

        public List<string> GetImageUrlsForProduct(string productId)
        {
            var images = this.context.ProductsImages.Where(x => x.ProductId == productId)
                .Select(x => x.Image)
                .ToList();
            var imageUrls = new List<string>();

            imageUrls.AddRange(images);

            return imageUrls;
        }

        public ProductImage GetMainImage(string productId)
        {
            var mainImage = this.context.ProductsImages
                .First(x => x.ProductId == productId & x.IsMain == true);

            return mainImage;
        }

        public bool ImagesAreRepeated(string mainImage, List<string> additionalImages)
        {
            var additionalImagesAreSame = additionalImages.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().Count() != additionalImages.Where(x => !string.IsNullOrWhiteSpace(x)).Count();
            return additionalImagesAreSame || additionalImages.Contains(mainImage);
        }

        public bool ProductImageExists(string imageUrl, bool hardCheck = false)
        {
            return context.Products
                .IgnoreAllQueryFilters(hardCheck)
                .SelectMany(x => x.Images)
                .Any(x => x.Image == imageUrl);
        }


        public bool ProductImageExists(string imageUrl, string excludedProductId, bool hardCheck = false)
        {
            return context.Products
                .IgnoreAllQueryFilters(hardCheck)
                .Where(x => x.Id != excludedProductId)
                .SelectMany(x => x.Images)
                .Any(x => x.Image == imageUrl);
        }

        public async Task<string> UploadImageAsync(IFormFile image, Product product)
        {
            var imageExtension = Path.GetExtension(image.FileName).TrimStart('.');
            var imageBlobName = $"{product.Name}_{product.Model}_{Guid.NewGuid()}.{imageExtension}";

            return await this.azureBlobService.UploadBlobAsync(image, GlobalConstants.ProductsImagesBlobContainer, imageBlobName);
        }

        public async Task<string> UploadImageAsync(Stream image, string imageFilePath)
        {
            var blobNameParts = imageFilePath.Split("/", 3);
            var blobNameProductAndModelPart = blobNameParts[1];
            var blobNameIdentificatorPart = blobNameParts[2];
            var blobName = $"{blobNameProductAndModelPart}_{blobNameIdentificatorPart}";

            return await this.azureBlobService.UploadBlobAsync(image, GlobalConstants.ProductsImagesBlobContainer, blobName);
        }

        public async Task ClearBlobImagesAsync()
        {
            await this.azureBlobService.ClearBlobContainerAsync(GlobalConstants.ProductsImagesBlobContainer);
        }

        public bool ValidImageExtension(IFormFile image)
        {
            var extension = Path.GetExtension(image.FileName).TrimStart('.');
            if (!GlobalConstants.AllowedExtensions.Any(x => extension.EndsWith(x)))
            {
                return false;
            }

            return true;
        }

        public ProductImage GetImageById(string id)
        {
            return this.context.ProductsImages.FirstOrDefault(x => x.Id == id);
        }

        public async Task EditImageAsync(EditProductImageUploadInputModel imageInfo, Product productToEdit)
        {
            //Get the image from the database
            var image = this.GetImageById(imageInfo.ModifiedImage.Id);

            if(image != null)
            {
                //If image upload is null, then just remove the image from the database
                if (imageInfo.ImageUpload != null)
                {
                    //Upload image to azure blob storage
                    var imageUri = await UploadImageAsync(imageInfo.ImageUpload, productToEdit);

                    //Set the image uri in the database
                    image.Image = imageUri;
                }
                else if (imageInfo.ImageUpload == null)
                {
                    this.context.ProductsImages.Remove(image);
                }
            }
            //If the image doesn't exist then simply add it to the database
            else
            {
                //Upload image to azure blob storage
                var imageUri = await UploadImageAsync(imageInfo.ImageUpload, productToEdit);

                //Create a new image and set the image uri
                var newImage = new ProductImage
                {
                    IsMain = false,
                    Product = productToEdit,
                    ProductId = productToEdit.Id,
                    Image = imageUri
                };

                await this.context.ProductsImages.AddAsync(newImage);
            }
        }
    }
}
