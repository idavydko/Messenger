
using System.Collections.Generic;

namespace Messenger.UI.Models.Facebook.API
{
    public class MessageResponse
    {
        public MessageAttachment attachment { get; set; }
        public List<QuickReply> quick_replies { get; set; }
        public string text { get; set; }
    }
}
