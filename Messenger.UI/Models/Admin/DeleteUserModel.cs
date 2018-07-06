
using System.ComponentModel.DataAnnotations;

namespace Messenger.UI.Models.Admin
{
    public class DeleteUserModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        public bool? IsSMSUser { get; set; }
    }
}
