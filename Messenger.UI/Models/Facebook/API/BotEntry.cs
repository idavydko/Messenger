
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Messenger.UI.Models.Facebook.API
{
    public class BotEntry
    {
        public string id { get; set; }
        public long time { get; set; }

        [Required]
        public List<BotMessageReceivedRequest> messaging { get; set; }
    }
}
