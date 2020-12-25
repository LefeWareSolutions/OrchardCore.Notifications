using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Email;
using OrchardCore.Notifications.ViewModels;

namespace OrchardCore.Notifications.Controllers
{
    [Admin]
    public class SendNotificationsController : Controller
    {
        private readonly ISmtpService _smtpService;
        private readonly IAuthorizationService _authorizationService;
        private readonly INotifier _notifier;
        private readonly IHtmlLocalizer H;

        public SendNotificationsController(IAuthorizationService authorizationService, INotifier notifier, ISmtpService smtpService,
            IHtmlLocalizer<SendNotificationsController> h)
        {
            _authorizationService = authorizationService;
            _notifier = notifier;
            _smtpService = smtpService;
            H = h;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!await _authorizationService.AuthorizeAsync(User, NotificationsPermissions.SendNotifications))
            {
                return Forbid();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendNotificationPost(SendNotificationViewModel model)
        {
            if (!await _authorizationService.AuthorizeAsync(User, NotificationsPermissions.SendNotifications))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                var message = CreateMessageFromViewModel(model);

                //TODO: Keep audit trail?
                var result = await _smtpService.SendAsync(message);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("*", error.ToString());
                    }
                }
                else
                {
                    _notifier.Success(H["Message sent successfully"]);

                    return RedirectToAction("MessageSentSuccess");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult MessageSentSuccess()
        {
            return View();
        }



        private MailMessage CreateMessageFromViewModel(SendNotificationViewModel notificationSettings)
        {
            var message = new MailMessage
            {
                To = notificationSettings.To,
                Bcc = notificationSettings.Bcc,
                Cc = notificationSettings.Cc,
            };

            if (!String.IsNullOrWhiteSpace(notificationSettings.Sender))
            {
                message.Sender = notificationSettings.Sender;
            }

            if (!String.IsNullOrWhiteSpace(notificationSettings.Subject))
            {
                message.Subject = notificationSettings.Subject;
            }

            if (!String.IsNullOrWhiteSpace(notificationSettings.Body))
            {
                message.Body = notificationSettings.Body;
            }

            return message;
        }

    }
}
