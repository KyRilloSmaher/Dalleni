using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Dalleni.Infrastructure.ExternalServices
{
    public class CloudinaryImageUploaderService : IImageUploaderService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryImageUploaderService(IOptions<CloudinarySettings> settings)
        {
            var s = settings.Value ?? throw new ArgumentNullException(nameof(settings));

            var account = new Account(
                s.CloudName,
                s.ApiKey,
                s.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        // ---------------------------
        // SIMPLE UPLOAD (STREAM)
        // ---------------------------
        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            if (fileStream == null)
                throw new ArgumentNullException(nameof(fileStream));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            var publicId = Path.GetFileNameWithoutExtension(fileName);

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                PublicId = publicId,
                Overwrite = true,
                Transformation = new Transformation()
                    .Quality("auto")
                    .FetchFormat("auto")
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
                throw new Exception(result.Error.Message);

            return result.SecureUrl.ToString();
        }

        // ---------------------------
        // UPLOAD (IFormFile) 
        // ---------------------------
        public async Task<UploadedFileResult> UploadImageAsync(IFormFile imageFile, ImageFolder imageFolder)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Image file is required");

            var folder = GetFolderName(imageFolder);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";

            using var stream = imageFile.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                PublicId = $"{folder}/{Path.GetFileNameWithoutExtension(fileName)}",
                Folder = folder,
                Overwrite = true,
                Transformation = new Transformation()
                    .Quality("auto")
                    .FetchFormat("auto")
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
                throw new Exception(result.Error.Message);

            return new UploadedFileResult
            {
                Url = result.SecureUrl.ToString(),
                FileId = result.PublicId,
                Provider = "Cloudinary"
            };
        }

        // ---------------------------
        // MULTIPLE UPLOAD
        // ---------------------------
        public async Task<IEnumerable<UploadedFileResult>> UploadMultipleImagesAsync(
            IEnumerable<IFormFile> imageFiles,
            ImageFolder imageFolder)
        {
            if (imageFiles == null || !imageFiles.Any())
                throw new ArgumentException("At least one image is required");

            var tasks = imageFiles.Select(f => UploadImageAsync(f, imageFolder));
            return await Task.WhenAll(tasks);
        }

        // ---------------------------
        // DELETE BY PUBLIC ID
        // ---------------------------
        public async Task<bool> DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                throw new ArgumentNullException(nameof(publicId));

            var result = await _cloudinary.DestroyAsync(new DeletionParams(publicId));

            return result.Result == "ok";
        }

        // ---------------------------
        // DELETE BY URL
        // ---------------------------
        public async Task<bool> DeleteImageByUrlAsync(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentNullException(nameof(imageUrl));

            var publicId = ExtractPublicId(imageUrl);
            return await DeleteImageAsync(publicId);
        }

        // ---------------------------
        // GET IMAGE URL
        // ---------------------------
        public async Task<string> GetImageUrlAsync(string imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName))
                throw new ArgumentNullException(nameof(imageName));

            var result = await _cloudinary.GetResourceAsync(imageName);
            return result.SecureUrl;
        }

        // ---------------------------
        // HELPERS
        // ---------------------------
        private string GetFolderName(ImageFolder type)
        {
            return type switch
            {
                ImageFolder.profiles => "UserProfilesImages",
                ImageFolder.logos => "Logos",
                _ => "others"
            };
        }

        private string ExtractPublicId(string url)
        {
            try
            {
                var uri = new Uri(url);
                var segments = uri.AbsolutePath.Split('/');

                var file = segments[^1];
                var publicId = Path.GetFileNameWithoutExtension(file);

                if (segments.Length > 2)
                {
                    var folder = string.Join("/", segments.Skip(1).Take(segments.Length - 2));
                    return $"{folder}/{publicId}";
                }

                return publicId;
            }
            catch
            {
                return null;
            }
        }
    }
}