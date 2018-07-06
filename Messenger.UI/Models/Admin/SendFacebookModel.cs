
using System.ComponentModel.DataAnnotations;

namespace Messenger.UI.Models.Admin
{
    public class SendFacebookModel
    {
        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Message { get; set; }
    }
}
