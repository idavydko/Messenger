
using System.Collections.Generic;

namespace Messenger.UI.Models.Facebook.API
{
    public class PayloadElements
    {
        public string title { get; set; }
        public string image_url { get; set; }
        public string subtitle { get; set; }
        public List<ResponseButtons> buttons { get; set; }
        public string item_url { get; set; }
        public int? quantity { get; set; }
        public decimal? price { get; set; }
        public string currency { get; set; }
    }
}
