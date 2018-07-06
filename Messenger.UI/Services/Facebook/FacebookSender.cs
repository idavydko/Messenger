using Messenger.UI.Models.Facebook;
using Messenger.UI.Models.Facebook.API;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.UI.Services.Facebook
{
    public class FacebookSender : IFacebookSender
    {
        private readonly FacebookConfig _configs;
        private readonly ILogger<FacebookSender> _logger;
        public FacebookSender(IOptions<FacebookConfig> configs, ILogger<FacebookSender> logger)
        {
            _configs = configs.Value;
            _logger = logger;
        }

        public async Task<string> GetUsername(string userId)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var url = string.Format(Messages.FbGetUserDataUrl, _configs.GraphApiUrl, userId, _configs.AccessToken);
                    var response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();

                    var user = JsonConvert.DeserializeObject<FacebookUserSlim>(json);
                    return $"{user.First_Name} {user.Last_Name}";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"GetUsername {userId} failed");
                }
            }

            return string.Empty;
        }

        public async Task<Result> HandleMessage(string psId, BotMessage message)
        {
            var response = new MessageResponse();
            if (!string.IsNullOrWhiteSpace(message.text))
            {
                response.text = $"`You sent the message: {message.text}.'";
            }
            else if(message.attachments != null && message.attachments.Any())
            {
                response.attachment = message.attachments[0];
            }

            return await SendMessage(psId, response);
        }

        public async Task<Result> HandlePostback(string psId, BotPostback postback)
        {
            if(!string.IsNullOrWhiteSpace(postback.payload))
            {
                var response = new MessageResponse();

                if(postback.payload == "yes")
                {
                    response.text = "Thanks!";
                }
                else if(postback.payload == "no")
                {
                    response.text = "No forgiveness";
                }

                return await SendMessage(psId, response);
            }

            return new Result();
        }

        public async Task<IEnumerable<Result>> SendMessage(IEnumerable<string> destinations, MessageResponse message)
        {
            var results = new List<Result>();

            foreach (var destination in destinations)
            {
                try
                {
                    results.Add(await SendMessage(destination, message));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, destination, message);
                    results.Add(new Result());
                }
            }

            return results;
        }

        public async Task<Result> SendMessage(string psId, MessageResponse message)
        {
            var entry = new BotMessageResponse
            {
                recipient = new BotUser { id = psId },
                message = message
            };

            var url = string.Format(Messages.FbSendMessageUrl, _configs.GraphApiUrl, _configs.AccessToken);

            return await PostRaw(url, entry);
        }

        private async Task<Result> PostRaw(string url, BotMessageResponse data)
        {
            var result = new Result();
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(data, 
                                                           Formatting.None,
                                                           new JsonSerializerSettings
                                                           {
                                                               NullValueHandling = NullValueHandling.Ignore
                                                           });

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);

                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    result.Success = response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.Message;
                    _logger.LogError(ex, $"PostRaw failed {url}| data {data}");
                }
            }

            return result;
        }
    }
}
