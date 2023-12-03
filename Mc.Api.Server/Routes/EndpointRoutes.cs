namespace Mc.Api.Server
{
    /// <summary>
    /// The endpoint routes
    /// </summary>
    public class EndpointRoutes
    {
        #region Article

        /// <summary>
        /// The route to the CreateArticle endpoint
        /// </summary>
        public const string CreateArticle = "api/article/create";

        /// <summary>
        /// The route to the FetchArticles endpoint
        /// </summary>
        public const string FetchArticles = "api/articles/fetch";

        /// <summary>
        /// The route to the FetchArticle endpoint
        /// </summary>
        public const string FetchArticle = "api/article/fetch/{id}";

        /// <summary>
        /// The route to the UpdateArticle endpoint
        /// </summary>
        public const string UpdateArticle = "api/article/update/{id}";

        /// <summary>
        /// The route to the DeleteArticle endpoint
        /// </summary>
        public const string DeleteArticle = "api/article/delete/{id}";

        #endregion

        #region Author

        /// <summary>
        /// The route to the CreateAuthor endpoint
        /// </summary>
        public const string CreateAuthor = "api/author/create";

        /// <summary>
        /// The route to the FetchAuthors endpoint
        /// </summary>
        public const string FetchAuthors = "api/author/fetch";

        /// <summary>
        /// The route to the FetchAuthor endpoint
        /// </summary>
        public const string FetchAuthor = "api/author/fetch/{id}";

        /// <summary>
        /// The route to the UpdateAuthor endpoint
        /// </summary>
        public const string UpdateAuthor = "api/author/update/{id}";

        /// <summary>
        /// The route to the DeleteAuthor endpoint
        /// </summary>
        public const string DeleteAuthor = "api/author/delete/{id}";

        #endregion

        #region Tag

        /// <summary>
        /// The route to the CreateTag endpoint
        /// </summary>
        public const string CreateTag = "api/tag/create";

        /// <summary>
        /// The route to the FetchTags endpoint
        /// </summary>
        public const string FetchTags = "api/tag/fetch";

        /// <summary>
        /// The route to the FetchTag endpoint
        /// </summary>
        public const string FetchTag = "api/tag/fetch/{id}";

        /// <summary>
        /// The route to the UpdateTag endpoint
        /// </summary>
        public const string UpdateTag = "api/tag/update/{id}";

        /// <summary>
        /// The route to the DeleteTag endpoint
        /// </summary>
        public const string DeleteTag = "api/tag/delete/{id}";

        #endregion
    }
}