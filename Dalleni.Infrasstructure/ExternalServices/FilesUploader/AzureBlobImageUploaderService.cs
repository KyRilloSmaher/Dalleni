using Azure.Storage.Blobs;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Dalleni.Infrastructure.ExternalServices.FilesUploader
{


    public class AzureBlobImageUploaderService : IImageUploaderService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private const string ContainerName = "images";

        public AzureBlobImageUploaderService(IConfiguration config)
        {
            _blobServiceClient = new BlobServiceClient(
                config["AzureBlobStorage"]);
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(ContainerName);
            await container.CreateIfNotExistsAsync();

            var blobName = $"{Guid.NewGuid()}_{fileName}";
            var blob = container.GetBlobClient(blobName);

            await blob.UploadAsync(fileStream, overwrite: true);

            return blob.Uri.ToString();
        }

        public async Task<UploadedFileResult> UploadImageAsync(IFormFile imageFile, ImageFolder imageType)
        {
            var container = _blobServiceClient.GetBlobContainerClient(ContainerName);
            await container.CreateIfNotExistsAsync();

            var blobName = $"{imageType}/{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var blob = container.GetBlobClient(blobName);

            using var stream = imageFile.OpenReadStream();
            await blob.UploadAsync(stream, overwrite: true);

            return new UploadedFileResult
            {
                Url = blob.Uri.ToString(),
                FileId = blobName,
                Provider = "Azure"
            };
        }

        public async Task<IEnumerable<UploadedFileResult>> UploadMultipleImagesAsync(
            IEnumerable<IFormFile> imageFiles, ImageFolder imageType)
        {
            var tasks = imageFiles.Select(f => UploadImageAsync(f, imageType));
            return await Task.WhenAll(tasks);
        }

        public async Task<bool> DeleteImageAsync(string fileId)
        {
            var container = _blobServiceClient.GetBlobContainerClient(ContainerName);
            var blob = container.GetBlobClient(fileId);

            var result = await blob.DeleteIfExistsAsync();
            return result.Value;
        }

        public async Task<bool> DeleteImageByUrlAsync(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return false;

            if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri))
                return false;

            var segments = uri.Segments;
            var fileId = string.Join("", segments.Skip(2));

            return await DeleteImageAsync(fileId);
        }

        public Task<string> GetImageUrlAsync(string fileId)
        {
            var blob = _blobServiceClient
                .GetBlobContainerClient(ContainerName)
                .GetBlobClient(fileId);

            return Task.FromResult(blob.Uri.ToString());
        }
    }
}
