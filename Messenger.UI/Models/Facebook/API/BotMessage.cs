
using System.Collections.Generic;

namespace Messenger.UI.Models.Facebook.API
{
    public class BotMessage
    {
        public string mid { get; set; }
        public List<MessageAttachment> attachments { get; set; }
        public long seq { get; set; }
        public string text { get; set; }
        public QuickReply quick_reply { get; set; }
    }
}
