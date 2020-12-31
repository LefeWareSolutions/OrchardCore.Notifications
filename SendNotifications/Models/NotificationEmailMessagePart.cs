using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Notifications.SendNotifications.Models
{
    public class NotificationEmailMessagePart : ContentPart
    {
        public TextField From { get; set; }
        public TextField Cc { get; set; }
        public TextField Bcc { get; set; }
        public TextField Subject { get; set; }
        public TextField Body { get; set; }
    }
}