using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Messenger.UI.Services;
using Messenger.UI.Models.Admin;
using Messenger.UI.Models.Facebook.API;

namespace Messenger.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly IAdminService _service;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService service,
                               ILogger<AdminController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.GetUsersAsync());
        }

        public async Task<IActionResult> Success()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToAll(SendSmsModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var results = await _service.SendToAll(model.Message);

                if (results.All(x => x.Success))
                {
                    return View("Success");
                }
                else
                {
                    ModelState.AddModelError("", Messages.UnsuccessfulOperation);
                }
            }

            return View("SmsForm", model);
        }

        [HttpPost]
        public async Task<IActionResult> SendToAllMessenger(MessageResponse model)
        {
            if (model != null && ModelState.IsValid)
            {
                var results = await _service.SendToMessenger(model);

                if (results.All(x => x.Success))
                {
                    return View("Success");
                }
                else
                {
                    ModelState.AddModelError("", Messages.UnsuccessfulOperation);
                }
            }

            return View("FacebookForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteUserModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var success = await _service.DeleteAsync(model.UserId, model.IsSMSUser.Value);

                if (success)
                {
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError("", Messages.NoUser);
                }
            }

            return BadRequest(ModelState);
        }
    }
}