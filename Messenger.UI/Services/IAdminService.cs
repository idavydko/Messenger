
using Messenger.UI.Models.Admin;
using Messenger.UI.Models.Facebook.API;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messenger.UI.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<IEnumerable<Result>> SendToAll(string message);

        Task<IEnumerable<Result>> SendToSmsUsers(string message);

        Task<IEnumerable<Result>> SendToMessenger(MessageResponse message);

        Task<bool> DeleteAsync(int userId, bool isSMSUser);
    }
}
