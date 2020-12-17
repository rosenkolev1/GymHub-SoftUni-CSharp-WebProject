using GymHub.Data.Models;
using System;
using System.Collections.Generic;
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
    }
}
