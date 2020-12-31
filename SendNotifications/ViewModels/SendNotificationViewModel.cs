using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.Notifications.SendNotifications;

namespace OrchardCore.Notifications.ViewModels
{
    public class SendNotificationViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string To { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Sender { get; set; }

        public string Bcc { get; set; }

        public string Cc { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public List<NotificationRecepientGroupViewModel> NotificationRecepientGroups { get; set; } = new List<NotificationRecepientGroupViewModel>();
    }
}
