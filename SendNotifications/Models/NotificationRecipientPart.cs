using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;

namespace OrchardCore.Notifications.SendNotifications.Models
{
    public class NotificationRecipientPart : ContentPart
    {
        public TextField Emails { get; set; }
        public UserPickerField UserEmails { get; set; }
    }
}