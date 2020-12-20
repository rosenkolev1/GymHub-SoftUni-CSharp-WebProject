using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.AzureBlobService
{
    public interface IAzureBlobService
    {
        public Task<string> UploadBlobAsync(IFormFile file, string blobContainerName, string blobName);
        public Task ClearBlobContainerAsync(string blobContainerName);
        public Task<string> UploadBlobAsync(Stream fileStream, string blobContainerName, string blobName);
        public Task DeleteAllBlobsAsync(string containerName, List<string> blobUrls);
        public List<string> GetAllBlobUrls (string containerName);
    }
}
