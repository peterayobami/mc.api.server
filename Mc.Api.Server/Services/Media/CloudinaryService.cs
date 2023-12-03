using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Mc.Api.Server
{
    public class CloudinaryService
    {
        /// <summary>
        /// DI instance of ILogger
        /// </summary>
        private readonly ILogger<CloudinaryService> _logger;

        /// <summary>
        /// The scoped instance of cloudinary
        /// </summary>
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(ILogger<CloudinaryService> logger, Cloudinary cloudinary)
        {
            _logger = logger;
            _cloudinary = cloudinary;
        }

        /// <summary>
        /// Handles uploading images to cloudinary
        /// </summary>
        /// <param name="base64ImgUrl"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public async Task<ImageUploadResultApiModel> UploadAsync(string base64ImgUrl, string imageName = null, string uploadPreset = CloudinaryUploadPresets.Default)
        {
            // Create instance of image upload result api model
            ImageUploadResultApiModel result = new();

            try
            {
                // If the base 64 image url has content
                if (!string.IsNullOrWhiteSpace(base64ImgUrl))
                {
                    // Transform the data to bytes
                    byte[] data = Convert.FromBase64String(base64ImgUrl);

                    // If no image name was passed
                    if (imageName == null)
                    {
                        imageName = Guid.NewGuid().ToString();
                    }

                    // Create instance of image upload result
                    var uploadResult = new ImageUploadResult();

                    using (var stream = new MemoryStream(data))
                    {
                        // Create instance of image upload params
                        // Note : Use the image params in cloudinary dashboard
                        var uploadParams = new ImageUploadParams
                        {
                            UploadPreset = uploadPreset,
                            File = new FileDescription(imageName, stream)
                        };

                        // Upload the image to cloudinary
                        uploadResult = await _cloudinary.UploadAsync(uploadParams);

                        // If it was successful
                        if (uploadResult.Error == null)
                        {
                            // Set the image upload id and url
                            result.ImageUrl = uploadResult.Url.ToString();

                            result.ImageId = uploadResult.PublicId;
                        }
                        else
                        {
                            // Set the errors
                            result.ErrorMessage = "An error occurred while uploading to cloudinary";

                            result.Errors = uploadResult.Error;

                            // Return the result
                            return result;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError("An error occurred. Details : {error}", ex.Message);

                // Return the error
                return new ImageUploadResultApiModel
                {
                    ErrorMessage = ex.Message
                };
            }

            return result;
        }

        /// <summary>
        /// Handles deleting images from cloudinary
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public async Task DeleteFromCloudinaryAsync(string imageId)
        {
            try
            {
                // Check if an image id was passed
                if (!string.IsNullOrWhiteSpace(imageId))
                {
                    // Destroy the image
                    await _cloudinary.DestroyAsync(new DeletionParams(imageId));
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError("An error occurred. Details: {error}", ex.Message);
            }
        }
    }
}
