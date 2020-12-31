using System.Collections.Generic;

namespace OrchardCore.Notifications.ViewModels
{
    public class NotificationSettingsViewModel
    {
        public IList<NotificationEntry> Notifications { get; set; }
        public dynamic Pager { get; set; }
    }

    public class NotificationEntry
    {
        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
    }
}
