
using System.ComponentModel.DataAnnotations;

namespace Messenger.UI.Models.Facebook.API
{
    public class BotMessageReceivedRequest
    {
        [Required]
        public BotUser sender { get; set; }

        public BotUser recipient { get; set; }
        public string timestamp { get; set; }

        [Required]
        public BotMessage message { get; set; }

        public BotPostback postback { get; set; }
    }
}
