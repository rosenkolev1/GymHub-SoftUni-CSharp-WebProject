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

        public List<string> GetImageUrlsForProduct(string productId)
        {
            var something = this.context.Products.Where(x => x.Id == productId)
                .Select(x => new
                {
                    MainImage = x.MainImage,
                    AdditionalImages = x.AdditionalImages.Select(x => x.Image)
                })
                .First();
            var imageUrls = new List<string>();
            imageUrls.Add(something.MainImage);
            imageUrls.AddRange(something.AdditionalImages);

            return imageUrls;
        }

        public bool ImagesAreRepeated(string mainImage, List<string> additionalImages)
        {
            var additionalImagesAreSame = additionalImages.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().Count() != additionalImages.Where(x => !string.IsNullOrWhiteSpace(x)).Count();
            return additionalImagesAreSame || additionalImages.Contains(mainImage);
        }

        public bool ProductImageExists(string imageUrl, bool hardCheck = false)
        {
            return context.Products.IgnoreAllQueryFilters(hardCheck).Any(x => x.MainImage == imageUrl) ||
                context.ProductsImages.IgnoreAllQueryFilters(hardCheck).Any(x => x.Image == imageUrl);
        }


        public bool ProductImageExists(string imageUrl, string excludedProductId, bool hardCheck = false)
        {
            return context.Products.IgnoreAllQueryFilters(hardCheck).Where(x => x.Id != excludedProductId).Any(x => x.MainImage == imageUrl) ||
                context.ProductsImages.IgnoreAllQueryFilters(hardCheck).Where(x => x.ProductId != excludedProductId).Any(x => x.Image == imageUrl);
        }
    }
}
