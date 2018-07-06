
using System;
using System.ComponentModel.DataAnnotations;

namespace Messenger.UI.Models.Sms
{
    public class SmsUser
    {
        public int Id { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
