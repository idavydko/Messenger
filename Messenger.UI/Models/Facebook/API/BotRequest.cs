
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Messenger.UI.Models.Facebook.API
{
    public class BotRequest
    {
        public string @object { get; set; }

        [Required]
        public List<BotEntry> entry { get; set; }
    }
}
