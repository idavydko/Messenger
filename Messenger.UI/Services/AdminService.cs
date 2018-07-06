using Messenger.UI.Data.Contracts;
using Messenger.UI.Models.Admin;
using Messenger.UI.Models.Facebook.API;
using Messenger.UI.Services.Facebook;
using Messenger.UI.Services.Sms;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.UI.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _repository;
        private readonly ISmsSender _smsSender;
        private readonly IFacebookSender _fbSender;
        private readonly ILogger<AdminService> _logger;

        public AdminService(IUserRepository repository,
                            ISmsSender smsSender,
                            IFacebookSender fbSender,
                            ILogger<AdminService> logger)
        {
            _repository = repository;
            _smsSender = smsSender;
            _fbSender = fbSender;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _repository.GetUsersAsync();
        }

        public async Task<IEnumerable<Result>> SendToAll(string message)
        {
            var results = new List<Result>();

            results.AddRange(await SendToSmsUsers(message));

            var fbMessage = new MessageResponse { text = message };

            results.AddRange(await SendToMessenger(fbMessage));

            return results;
        }

        public async Task<IEnumerable<Result>> SendToSmsUsers(string message)
        {
            var smsUsers = await _repository.GetSmsUsersAsync();
            var results = new List<Result>();

            if (smsUsers.Any())
            {
                var destinations = smsUsers.Select(x => x.PhoneNumber).ToList();
                results.AddRange(await _smsSender.SendSMS(destinations, message));
            }
            else
            {
                _logger.LogError(Messages.NoSmsUser, message);
                results.Add(new Result() { ErrorMessage = Messages.NoSmsUser });
            }

            return results;
        }

        public async Task<IEnumerable<Result>> SendToMessenger(MessageResponse message)
        {
            var fbUsers = await _repository.GetFacebookUsersAsync();
            var results = new List<Result>();
            
            if (fbUsers.Any())
            {
                var destinations = fbUsers.Select(x => x.PSID).ToList();
                results.AddRange(await _fbSender.SendMessage(destinations, message));
            }
            else
            {
                _logger.LogError(Messages.NoFacebookUser, message);
                results.Add(new Result() { ErrorMessage = Messages.NoFacebookUser });
            }

            return results;
        }

        public async Task<bool> DeleteAsync(int userId, bool isSMSUser)
        {
            return await _repository.DeleteAsync(userId, isSMSUser);
        }
    }
}
