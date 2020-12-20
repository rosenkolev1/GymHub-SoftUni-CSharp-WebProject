using GymHub.Common;
using GymHub.Services.ServicesFolder.AzureBlobService;
using GymHub.Services.ServicesFolder.ProductImageService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services.CronJobs
{
    public class DeleteProductsImagesBlobs
    {
        private readonly IProductImageService productImageService;
        private readonly IAzureBlobService azureBlobService;

        public DeleteProductsImagesBlobs(IProductImageService productImageService, IAzureBlobService azureBlobService)
        {
            this.productImageService = productImageService;
            this.azureBlobService = azureBlobService;
        }

        public async Task DeleteUnusedProducstImagesBlobsFromAzureBlobStorageAsyncJob()
        {
            var allProductsImagesBlobsUrlsFromDatabase = this.productImageService.GetAllImagesBlobsUrlsFromDatabase();
            var allProductsImagesBlobsUrlsFromAzureStorage = this.productImageService.GetAllImagesBlobsUrlsFromAzureStorage();

            var deletedImagesUrls = new List<string>();

            foreach (var productImageUrlFromAzure in allProductsImagesBlobsUrlsFromAzureStorage)
            {
                if(allProductsImagesBlobsUrlsFromDatabase.Contains(productImageUrlFromAzure) == false)
                {
                    deletedImagesUrls.Add(productImageUrlFromAzure);
                }
            }

            await this.azureBlobService.DeleteAllBlobsAsync(GlobalConstants.ProductsImagesBlobContainer, deletedImagesUrls);
        }
    }
}
