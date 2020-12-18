using Azure.Storage.Blobs;
using GymHub.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.AzureBlobService
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobServiceClient blobServiceClient;

        public AzureBlobService(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadBlobAsync(IFormFile file, string blobContainerName, string blobName)
        {
            var mainImage = file;

            // Open the main image and upload its data
            using Stream uploadFileStream = mainImage.OpenReadStream();

            var blobContainerClient = this.blobServiceClient.GetBlobContainerClient(blobContainerName);
            var blobContentInfo = await blobContainerClient.UploadBlobAsync(blobName, uploadFileStream);

            uploadFileStream.Close();

            var blobClient = blobContainerClient.GetBlobClient(blobName);

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<string> UploadBlobAsync(Stream fileStream, string blobContainerName, string blobName)
        {
            //var mainImage = file;

            //// Open the main image and upload its data
            //using Stream uploadFileStream = mainImage.OpenReadStream();

            var blobContainerClient = this.blobServiceClient.GetBlobContainerClient(blobContainerName);
            await blobContainerClient.UploadBlobAsync(blobName, fileStream);

            fileStream.Close();

            var blobClient = blobContainerClient.GetBlobClient(blobName);

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task ClearBlobContainerAsync(string blobContainerName)
        {
            var blobContainerClient = this.blobServiceClient.GetBlobContainerClient(blobContainerName);
            var blobs = blobContainerClient.GetBlobs();

            foreach (var blob in blobs)
            {
                var blobClient = blobContainerClient.GetBlobClient(blob.Name);
                await blobClient.DeleteAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);
            }
        }
    }
}
