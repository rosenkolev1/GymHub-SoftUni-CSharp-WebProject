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

        public async Task DeleteAllBlobsAsync(string containerName, List<string> blobUrls)
        {
            var blobContainerClient = this.blobServiceClient.GetBlobContainerClient(containerName);
            var allBlobClients = blobContainerClient.GetBlobs()
                .Select(x => blobContainerClient.GetBlobClient(x.Name))
                .ToList();

            foreach (var blobUrl in blobUrls)
            {
                var blobClientToDelete = allBlobClients.First(x => x.Uri.AbsoluteUri == blobUrl);
                await blobClientToDelete.DeleteIfExistsAsync();
            }
        }

        public List<string> GetAllBlobUrls(string containerName)
        {
            var blobContainerClient = this.blobServiceClient.GetBlobContainerClient(containerName);

            var blobNames = blobContainerClient.GetBlobs()
                .Select(x => x.Name).ToList();

            var blobUrls = new List<string>();

            foreach (var blobName in blobNames)
            {
                var blobClient = blobContainerClient.GetBlobClient(blobName);
                blobUrls.Add(blobClient.Uri.AbsoluteUri);
            }

            return blobUrls;
        }
    }
}
