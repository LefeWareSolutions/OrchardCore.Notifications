using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchardCore.Notifications.NotificationSettings.ViewModels
{
    public class NotificationTemplate
    {
        public int WorkflowTypeId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string TemplateBody { get; set; }
        public string ReturnUrl { get; set; }
    }
}
