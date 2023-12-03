using System.ComponentModel.DataAnnotations;

namespace Mc.Api.Server
{
    /// <summary>
    /// The credentials to create an article
    /// </summary>
    public class ArticleCredentials
    {
        /// <summary>
        /// The title of the article
        /// </summary>
        [Required(ErrorMessage = "The title of an article is required")]
        public string Title { get; set; }

        /// <summary>
        /// The description of the article
        /// </summary>
        [Required(ErrorMessage = "The description of an article is required")]
        public string Description { get; set; }

        /// <summary>
        /// The content of the article
        /// </summary>
        [Required(ErrorMessage = "The content of an article is required")]
        public string Content { get; set; }

        /// <summary>
        /// The author id for this article
        /// </summary>
        [Required(ErrorMessage = "The author id of an article is required")]
        public string AuthorId { get; set; }

        /// <summary>
        /// The tags for this article
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// The base64 url-encoded caption for this article
        /// </summary>
        [Required(ErrorMessage = "The caption of an article is required")]
        public string Caption { get; set; }
    }
}