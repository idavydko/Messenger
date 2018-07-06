
using System.ComponentModel.DataAnnotations;

namespace Messenger.UI.Models.Facebook.API
{
    public class BotMessageResponse
    {
        [Required]
        public BotUser recipient { get; set; }
        [Required]
        public MessageResponse message { get; set; }
    }
}
