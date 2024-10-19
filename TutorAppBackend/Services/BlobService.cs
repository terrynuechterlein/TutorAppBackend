using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;

namespace TutorAppBackend.Services
{
    public class BlobService
    {
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(IConfiguration configuration)
        {
            _configuration = configuration;

            var connectionString = _configuration["AzureStorage:ConnectionString"];
            Console.WriteLine($"AzureStorage:ConnectionString = '{connectionString}'");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("AzureStorage:ConnectionString is not configured.");
            }

            _blobServiceClient = new BlobServiceClient(connectionString);
        }
        public string GenerateBlobSasToken(string containerNameKey, string blobName)
        {
            var containerName = _configuration[$"AzureStorage:{containerNameKey}"];
            Console.WriteLine($"Container name for key '{containerNameKey}': '{containerName}'");

            if (string.IsNullOrEmpty(containerName))
            {
                throw new InvalidOperationException($"Container name for key '{containerNameKey}' is not configured.");
            }
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerClient.Name,
                BlobName = blobClient.Name,
                Resource = "b", // b for blob
                StartsOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddHours(24) // Token valid for 24 hours
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasToken = blobClient.GenerateSasUri(sasBuilder).Query;

            return $"{blobClient.Uri}{sasToken}";
        }

        public async Task<string> SaveFileToAzureBlob(IFormFile file, string containerNameKey)
        {
            var containerName = _configuration[$"AzureStorage:{containerNameKey}"];
            Console.WriteLine($"Container name for key '{containerNameKey}': '{containerName}'");

            if (string.IsNullOrEmpty(containerName))
            {
                throw new InvalidOperationException($"Container name for key '{containerNameKey}' is not configured.");
            }
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            // Ensure that the container exists
            await containerClient.CreateIfNotExistsAsync();

            // Create a unique name for the blob
            var blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = containerClient.GetBlobClient(blobName);

            // Upload the file to Azure Blob Storage
            await using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            // Return the URL of the uploaded file
            return blobClient.Uri.ToString();
        }
    }
}
