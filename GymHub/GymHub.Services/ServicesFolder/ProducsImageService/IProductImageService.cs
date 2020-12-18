using GymHub.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.ProductImageService
{
    public interface IProductImageService
    {
        public bool ProductImageExists(string imageUrl, bool hardCheck = false);
        public bool ProductImageExists(string imageUrl, string excludedProductId, bool hardCheck = false);
        public Task AddProductImageAsync(ProductImage image);
        public bool ImagesAreRepeated(string mainImage, List<string> additionalImages);
        public List<string> GetImageUrlsForProduct(string productId);
        public ProductImage GetImageByPath(string imagePath);
        public ProductImage GetMainImage(string productId);
        public List<ProductImage> GetAdditionalImages(string productId);
        public bool ValidImageExtension(IFormFile image);

        public Task<string> UploadImageAsync(IFormFile image, Product product);
        public Task<string> UploadImageAsync(Stream image, string imageFilePath);
        public Task ClearBlobImagesAsync();
    }
}
