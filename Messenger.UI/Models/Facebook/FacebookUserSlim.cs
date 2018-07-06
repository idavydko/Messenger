
using System.ComponentModel.DataAnnotations;

namespace Messenger.UI.Models.Facebook
{
    public class FacebookUserSlim
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string First_Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Last_Name { get; set; }
    }
}
