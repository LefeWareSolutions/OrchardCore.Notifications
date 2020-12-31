using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Email;
using OrchardCore.Notifications.SendNotifications.Models;
using OrchardCore.Users.Indexes;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using YesSql;

namespace OrchardCore.Notifications.Handlers
{
    public class NotificationHandler: ContentPartHandler<Notification>
    {
        private readonly ISession _session;
        private readonly ISmtpService _smtpService;
        private readonly INotifier _notifier;
        private readonly IHtmlLocalizer H;
        public NotificationHandler(ISmtpService smtpService, INotifier notifier, IHtmlLocalizer<NotificationHandler> h, ISession session)
        {
            _smtpService = smtpService;
            _notifier = notifier;
            H = h;
            _session = session;
        }

        public override async Task PublishedAsync(PublishContentContext context, Notification notificationPart)
        {
            var contentItem = context.PublishingItem;
            var notificationRecipientPart = contentItem.As<NotificationRecipientPart>();
            var notificationEmailMessagePart = contentItem.As<NotificationEmailMessagePart>();

            //Get Recipients
            var userIds = notificationRecipientPart.UserEmails.UserIds;
            var emails = new StringBuilder();
            emails.Append(notificationRecipientPart.Emails);
            foreach(var userId in userIds)
            {
                var user = await _session.Query<User, UserIndex>(x => x.UserId == userId).FirstOrDefaultAsync();
                var email = user.Email;
                emails.Append(","+email);
            }

            var message = new MailMessage
            {
                To = emails.ToString(),
                Bcc = notificationEmailMessagePart.Bcc.Text,
                Cc = notificationEmailMessagePart.Cc.Text,
                Subject =  notificationEmailMessagePart.Subject.Text,
                Body = notificationEmailMessagePart.Body.Text
            };
            
            var result = await _smtpService.SendAsync(message);
            if (!result.Succeeded)
            {
                notificationPart.MessageStatus = NotificationStatus.Failed;
                foreach (var error in result.Errors)
                {
                    _notifier.Error(H["Message sent successfully"]);
                }
            }
            else
            {
                notificationPart.MessageStatus = NotificationStatus.Success;
                _notifier.Success(H["Message sent successfully"]);
            }

        }

        private MailMessage CreateMailMessageNotificationItem(string recipients, string bcc,  )
        {


            if (!String.IsNullOrWhiteSpace(notificationSettings.Sender))
            {
                message.Sender = notificationSettings.Sender;
            }

            if (!String.IsNullOrWhiteSpace(notificationSettings.Subject))
            {
                message.Subject = notificationSettings.Subject;
            }


            return message;
        }
    }
}
