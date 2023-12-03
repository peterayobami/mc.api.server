using System.ComponentModel.DataAnnotations.Schema;

namespace Mc.Api.Server
{
    /// <summary>
    /// The articles database table representational model
    /// </summary>
    public class ArticlesDataModel : BaseDataModel
    {
        /// <summary>
        /// The author id foreign key index
        /// </summary>
        public string AuthorId { get; set; }

        /// <summary>
        /// The title of this article
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description of this article
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The content of this article
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The image id of this article
        /// </summary>
        public string ImageId { get; set; }

        /// <summary>
        /// The image url of this article
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The tags of this article
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// The author relational entity model
        /// </summary>
        [ForeignKey(nameof(AuthorId))]
        public AuthorsDataModel Author { get; set; }
    }
}