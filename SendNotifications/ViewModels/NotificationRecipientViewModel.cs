using System.Collections.Generic;

namespace OrchardCore.Notifications.SendNotifications
{
    public class NotificationRecepientGroupViewModel
    {
        public string GroupName { get; set; }
        public List<NotificationRecipientViewModel> NotificationRecepients { get; set; } = new List<NotificationRecipientViewModel>();
    }

    public class NotificationRecipientViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Selected { get; set; }
    }
}