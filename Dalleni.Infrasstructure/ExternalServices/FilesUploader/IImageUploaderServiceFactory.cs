using Dalleni.Application.ExternalServicesAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dalleni.Infrastructure.ExternalServices.FilesUploader
{


    public class ImageUploaderServiceFactory : IImageUploaderServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public ImageUploaderServiceFactory(
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public IImageUploaderService Create()
        {
            var provider = _configuration["ImageStorageProvider"];

            return provider switch
            {
                "Azure" => _serviceProvider.GetRequiredService<AzureBlobImageUploaderService>(),
                "Cloudinary" => _serviceProvider.GetRequiredService<CloudinaryImageUploaderService>(),
                _ => throw new Exception("Invalid ImageStorageProvider configuration")
            };
        }
    }
}
