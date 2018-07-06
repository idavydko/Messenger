
using System.ComponentModel.DataAnnotations;

namespace Messenger.UI.Models.Sms
{
    public class SendCodeModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Code { get; set; }
    }
}
