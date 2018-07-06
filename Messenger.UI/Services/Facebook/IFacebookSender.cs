
using Messenger.UI.Models.Facebook.API;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messenger.UI.Services.Facebook
{
    public interface IFacebookSender
    {
        Task<Result> HandleMessage(string psId, BotMessage message);

        Task<Result> HandlePostback(string psId, BotPostback postback);

        Task<IEnumerable<Result>> SendMessage(IEnumerable<string> destinations, MessageResponse message);

        Task<Result> SendMessage(string psId, MessageResponse message);

        Task<string> GetUsername(string userId);
    }
}
