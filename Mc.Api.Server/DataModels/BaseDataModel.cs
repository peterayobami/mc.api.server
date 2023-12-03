namespace Mc.Api.Server
{
    public class BaseDataModel
    {
        /// <summary>
        /// The unique id for database entry
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The point in time record was created
        /// </summary>
        public DateTimeOffset DateCreated { get; set; }

        /// <summary>
        /// The point in time record was modified
        /// </summary>
        public DateTimeOffset DateModified { get; set; }
    }
}