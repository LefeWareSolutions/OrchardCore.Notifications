using OrchardCore.ContentManagement;

namespace OrchardCore.Notifications.SendNotifications.Models
{
    public class Notification : ContentPart
    {
        public NotificationStatus MessageStatus { get; set; }
        public NotificationType NotificationType { get; set; }
    }

    public enum NotificationStatus
    {
        Success,
        Failed
    }

    public enum NotificationType
    {
        Email
    }
}