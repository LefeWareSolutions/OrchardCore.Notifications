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
using OrchardCore.Modules;
using OrchardCore.Notifications.SendNotifications;
using OrchardCore.Notifications.ViewModels;
using OrchardCore.Users.Indexes;
using OrchardCore.Users.Models;
using YesSql;

namespace OrchardCore.Notifications.Controllers
{
    [Admin]
    [Feature(NotificationsConstants.Features.SendNotifications)]
    public class SendNotificationsController : Controller
    {
        private readonly ISmtpService _smtpService;
        private readonly IAuthorizationService _authorizationService;
        private readonly INotifier _notifier;
        private readonly IHtmlLocalizer H;
        private readonly ISession _session;

        public SendNotificationsController(IAuthorizationService authorizationService, INotifier notifier, ISmtpService smtpService,
            IHtmlLocalizer<SendNotificationsController> h, ISession session)
        {
            _authorizationService = authorizationService;
            _notifier = notifier;
            _smtpService = smtpService;
            H = h;
            _session = session;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!await _authorizationService.AuthorizeAsync(User, NotificationsPermissions.SendNotifications))
            {
                return Forbid();
            }

            //TODO: Make more generic or create an abstraction to allow different groups
            var sendNotificationViewModel = new SendNotificationViewModel();
            sendNotificationViewModel.NotificationRecepientGroups.Add(await GetUserGroup());

            return View(sendNotificationViewModel);
        }

        //TODO: Move to seperate class
        private async Task<NotificationRecepientGroupViewModel> GetUserGroup()
        {
            var userGroup =  new NotificationRecepientGroupViewModel();
            userGroup.GroupName = "Users";
            
            var allUsers = await _session.Query<User, UserIndex>().ListAsync();
            foreach (var user in allUsers)
            {
                userGroup.NotificationRecepients.Add(new NotificationRecipientViewModel()
                {
                    Email = user.Email,
                    Name = user.UserName,
                });
            }
            return userGroup;
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
