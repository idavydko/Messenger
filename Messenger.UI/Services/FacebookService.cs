using Messenger.UI.Data.Contracts;
using Messenger.UI.Services.Facebook;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Messenger.UI.Services
{
    public class FacebookService : IFacebookService
    {
        private readonly IUserRepository _repository;
        private readonly IFacebookSender _fbSender;
        private readonly FacebookConfig _configs;
        private readonly ILogger<FacebookService> _logger;

        public FacebookService(IUserRepository repository,
                                  IFacebookSender fbSender,
                                  IOptions<FacebookConfig> configs,
                                  ILogger<FacebookService> logger)
        {
            _repository = repository;
            _fbSender = fbSender;
            _configs = configs.Value;
            _logger = logger;
        }

        //public async Task<Result> SendMessage()
        //{

        //}
    }
}
