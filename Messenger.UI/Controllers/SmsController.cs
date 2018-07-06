using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Messenger.UI.Data.Contracts;
using Messenger.UI.Models.Sms;
using Messenger.UI.Services.Sms;
using System;

namespace Messenger.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class SmsController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly ISmsSender _smsSender;

        public SmsController(IUserRepository repository, ISmsSender smsSender)
        {
            _repository = repository;
            _smsSender = smsSender;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePhone(SmsUser model)
        {
            if (ModelState.IsValid)
            {
                await _smsSender.SendSMS(model.PhoneNumber, "Test code");

                return View("SendCode", new SendCodeModel { PhoneNumber = model.PhoneNumber });
            }
            
            return View("~/Views/Home/Index.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeModel model)
        {
            if (ModelState.IsValid)
            {
                var succeeded = await _repository.AddSmsUser(new SmsUser
                {
                    PhoneNumber = model.PhoneNumber,
                    CreatedDate = DateTime.Now
                });

                return View("ValidCode");
            }
            
            return View(model);
        }
    }
}