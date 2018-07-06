
using System.Collections.Generic;

namespace Messenger.UI.Models.Facebook.API
{
    public class MessageAttachmentPayLoad
    {
        public string url { get; set; }
        public string template_type { get; set; }
        public string top_element_style { get; set; }
        public List<PayloadElements> elements { get; set; }
        public List<ResponseButtons> buttons { get; set; }
        public string recipient_name { get; set; }
        public string order_number { get; set; }
        public string currency { get; set; }
        public string payment_method { get; set; }
        public string order_url { get; set; }
        public string timestamp { get; set; }
        public Address address { get; set; }
        public Summary summary { get; set; }
    }
}
