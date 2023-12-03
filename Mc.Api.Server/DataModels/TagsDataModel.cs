namespace Mc.Api.Server
{
    /// <summary>
    /// The article tags database table representational model
    /// </summary>
    public class TagsDataModel : BaseDataModel
    {
        /// <summary>
        /// The title of this tag
        /// </summary>
        public string Title { get; set; }
    }
}