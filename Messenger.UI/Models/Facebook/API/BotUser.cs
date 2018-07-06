
using System.ComponentModel.DataAnnotations;

namespace Messenger.UI.Models.Facebook.API
{
    public class BotUser
    {
        [Required]
        public string id { get; set; }
    }
}
