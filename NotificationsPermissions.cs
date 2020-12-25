using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Security.Permissions;

namespace OrchardCore.Notifications
{
    public class NotificationsPermissions : IPermissionProvider
    {
        public static readonly Permission SendNotifications = new Permission("SendNotifications", "Send Notifications");
        public static readonly Permission EditNotificationSettings = new Permission("EditNotificationSettings", "Edit Notification Settings");

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[]
            {
                SendNotifications,
                EditNotificationSettings
            }
            .AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { SendNotifications, EditNotificationSettings }
                },
            };
        }
    }
}
