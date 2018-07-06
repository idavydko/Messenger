using Messenger.UI.Models.Admin;
using Messenger.UI.Models.Facebook;
using Messenger.UI.Models.Sms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messenger.UI.Data.Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<IEnumerable<SmsUser>> GetSmsUsersAsync();

        Task<IEnumerable<FacebookUser>> GetFacebookUsersAsync();

        Task<bool> AddSmsUser(SmsUser user);

        Task<bool> AddFacebookUser(FacebookUser user);

        Task<bool> DeleteAsync(int userId, bool isSMSUser);
    }
}
