namespace Mc.Api.Server
{
    /// <summary>
    /// The authors database table representational model
    /// </summary>
    public class AuthorsDataModel : BaseDataModel
    {
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
        /// The photo id of this author
        /// </summary>
        public string PhotoId { get; set; }

        /// <summary>
        /// The photo url of this author
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// The articles this author has posted
        /// </summary>
        public List<ArticlesDataModel> Articles { get; set; }
    }
}