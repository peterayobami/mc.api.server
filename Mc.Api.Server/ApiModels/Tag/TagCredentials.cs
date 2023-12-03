using System.ComponentModel.DataAnnotations;

namespace Mc.Api.Server
{
    public class TagCredentials
    {
        /// <summary>
        /// The title of this tag
        /// </summary>
        [Required(ErrorMessage = "The title of the tag is required")]
        public string Title { get; set; }
    }
}