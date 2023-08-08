using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LibraryApplication.Helper;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;

namespace LibraryApplication.Services
{
    public class PhotoService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<PhotoService> _logger;

        /// <summary>
        /// PhotoService için constructor. Cloudinary ve logger servislerini ayarlar.
        /// </summary>
        public PhotoService(IOptions<CloudinarySettings> config, ILogger<PhotoService> logger)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
            _logger = logger;
        }

        /// <summary>
        /// Fotoğrafı Cloudinary'ye yükler ve yükleme sonucunu döndürür.
        /// </summary>
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            try
            {
                if (file.Length > 0)
                {
                    using var stream = file.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
                return uploadResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fotoğraf yüklenirken bir hata oluştu.");
                throw;
            }
        }
    }
}
