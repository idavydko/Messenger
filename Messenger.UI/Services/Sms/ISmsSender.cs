using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messenger.UI.Services.Sms
{
    public interface ISmsSender
    {
        Task<Result> SendSMS(string destination, string message);

        Task<IEnumerable<Result>> SendSMS(IEnumerable<string> destinations, string message);
    }
}
