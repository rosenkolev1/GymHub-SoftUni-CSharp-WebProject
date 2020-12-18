using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.ProductImageService
{
    public class ProductImageService : DeleteableEntityService, IProductImageService
    {
        public ProductImageService(ApplicationDbContext context)
            :base(context)
        {

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
    }
}
