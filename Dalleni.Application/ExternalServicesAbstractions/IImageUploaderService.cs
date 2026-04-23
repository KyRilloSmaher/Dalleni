using CloudinaryDotNet.Actions;
using Dalleni.Domin.Enums;
using Microsoft.AspNetCore.Http;

namespace Dalleni.Application.ExternalServicesAbstractions
{
    public class UploadedFileResult
    {
        public string Url { get; set; }
        public string FileId { get; set; } = default!; // cloudinary publicId OR blob name
        public string Provider { get; set; } = default!; // Azure / Cloudinary
        public string Error { get; set; } = default!;
    }
    public interface IImageUploaderService
    {
        Task<string> UploadImageAsync(Stream fileStream, string fileName);

        Task<UploadedFileResult> UploadImageAsync(IFormFile imageFile, ImageFolder imageType);

        Task<IEnumerable<UploadedFileResult>> UploadMultipleImagesAsync(IEnumerable<IFormFile> imageFiles, ImageFolder imageType);

        Task<bool> DeleteImageAsync(string fileId);

        Task<bool> DeleteImageByUrlAsync(string imageUrl);

        Task<string> GetImageUrlAsync(string fileId);
    }
}
