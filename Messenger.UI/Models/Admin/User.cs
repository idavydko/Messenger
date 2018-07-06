
using System;

namespace Messenger.UI.Models.Admin
{
    public class User
    {
        public int Id { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsSmsUser { get; set; }

        public bool IsFacebookUser { get; set; }
    }
}
