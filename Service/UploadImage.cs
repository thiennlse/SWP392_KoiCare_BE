using BusinessObject.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Service.Interface;

namespace Service
{
    public class UploadImage : IUploadImage
    {
        private readonly Cloudinary _cloudinary;

        public UploadImage(IOptions<CloudinarySetting> config)
        {
            var acc = new Account
            {
                Cloud = config.Value.CloudName,
                ApiKey = config.Value.ApiKey,
                ApiSecret = config.Value.ApiSecret
            };

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<String> SaveImage(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(200).Width(200)
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);

            }
            return uploadResult.SecureUrl.ToString();
        }
    }
}
