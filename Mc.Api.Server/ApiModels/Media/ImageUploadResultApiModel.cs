namespace Mc.Api.Server
{
    public class ImageUploadResultApiModel
    {
        /// <summary>
        /// Indicates whether the image upload is successful
        /// </summary>
        public bool Successful => ErrorMessage == null;

        /// <summary>
        /// Error messages if any
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The image id
        /// </summary>
        public string ImageId { get; set; }

        /// <summary>
        /// Image Url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The errors
        /// </summary>
        public object Errors { get; set; }
    }
}