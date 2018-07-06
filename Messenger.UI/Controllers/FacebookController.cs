using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Messenger.UI.Models.Facebook.API;
using Messenger.UI.Services.Facebook;
using Messenger.UI.Data.Contracts;
using Messenger.UI.Models.Facebook;
using Messenger.UI.Services;

namespace Messenger.UI.Controllers
{
    public class FacebookController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IFacebookSender _fbSender;
        private readonly FacebookConfig _configs;
        private readonly ILogger<FacebookController> _logger;

        public FacebookController(IUserRepository repository, 
                                  IFacebookSender fbSender,
                                  IOptions<FacebookConfig> configs,
                                  ILogger<FacebookController> logger)
        {
            _repository = repository;
            _fbSender = fbSender;
            _configs = configs.Value;
            _logger = logger;
        }

        [HttpPost]
        [Route("webhook")]
        public async Task<IActionResult> Webhook([FromBody]BotRequest model)
        {
            if (model != null && ModelState.IsValid)
            {
                foreach (var entry in model.entry)
                {
                    var webhookEvent = entry.messaging.FirstOrDefault();
                    var id = webhookEvent.sender.id;

                    var result = new Result();
                    if (webhookEvent.message != null)
                    {
                        result = await _fbSender.HandleMessage(webhookEvent.sender.id, webhookEvent.message);
                    }
                    else if (webhookEvent.postback != null)
                    {
                        result = await _fbSender.HandlePostback(webhookEvent.sender.id, webhookEvent.postback);
                    }

                    if (!result.Success)
                    {
                        _logger.LogError(Messages.MessageNotSend, model);
                        ModelState.AddModelError("", Messages.MessageNotSend);
                    }

                    var username = await _fbSender.GetUsername(id);

                    if(webhookEvent.postback == null && !string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(username))
                    {
                        var user = new FacebookUser
                        {
                            Username = username,
                            PSID = id,
                            CreatedDate = DateTime.Now
                        };

                        var success = await _repository.AddFacebookUser(user);

                        if (!success)
                        {
                            _logger.LogError(Messages.UserExists, user);
                        }
                    }
                }

                if (ModelState.ErrorCount == 0)
                    return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var VERIFY_TOKEN = _configs.PageVerifyToken;

            var query = Request.Query;

            if (query["hub.mode"].ToString() == "subscribe" &&
                query["hub.verify_token"].ToString() == VERIFY_TOKEN)
            {
                //var type = Request.QueryString["type"];
                var retVal = query["hub.challenge"].ToString();
                return Json(int.Parse(retVal));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}