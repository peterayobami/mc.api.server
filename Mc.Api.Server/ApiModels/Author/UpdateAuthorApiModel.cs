namespace Mc.Api.Server
{
    /// <summary>
    /// The API model to update author
    /// </summary>
    public class UpdateAuthorApiModel
    {
        /// <summary>
        /// The title of the author
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The author's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The author's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The author's display photo
        /// </summary>
        public string Photo { get; set; }
    }
}