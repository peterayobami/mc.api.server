namespace Mc.Api.Server
{
    /// <summary>
    /// The credentials to update an article
    /// </summary>
    public class UpdateArticleApiModel
    {
        /// <summary>
        /// The title of the article
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description of the article
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The content of the article
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The author id for this article
        /// </summary>
        public string AuthorId { get; set; }

        /// <summary>
        /// The tags for this article
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// The base64 url-encoded caption for this article
        /// </summary>
        public string Caption { get; set; }
    }
}