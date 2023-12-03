namespace Mc.Api.Server
{
    public class TagApiModel
    {
        /// <summary>
        /// The id of the tag
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The title of the tag
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The point in time tag was modified
        /// </summary>
        public DateTimeOffset DateModified { get; set; }
    }
}
