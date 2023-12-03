namespace Mc.Api.Server
{
    public class ArticleApiModel
    {
        /// <summary>
        /// The id of this article
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The title of this article
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description of this article
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The content of the article
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The author of this article
        /// </summary>
        public AuthorApiModel Author { get; set; }

        /// <summary>
        /// The point in time when article was modified
        /// </summary>
        public DateTimeOffset DateModified { get; set; }
    }
}