namespace Mc.Api.Server
{
    /// <summary>
    /// The author api model
    /// </summary>
    public class AuthorApiModel
    {
        /// <summary>
        /// The id of the author
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The title of the author
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The first name of the author
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the author
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The photo url of the author
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// The articles of this author
        /// </summary>
        public List<ArticleApiModel> Articles { get; set; }

        /// <summary>
        /// The point in time when author was modified
        /// </summary>
        public DateTimeOffset DateModified { get; set; }
    }
}
