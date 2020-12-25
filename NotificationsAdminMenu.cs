using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace OrchardCore.Notifications
{
    public class NotificationsAdminMenu : INavigationProvider
    {
        private readonly IStringLocalizer T;

        public NotificationsAdminMenu(IStringLocalizer<NotificationsAdminMenu> localizer)
        {
            T = localizer;
        }

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            builder.Add(T["Notifications"], "7", settings => settings
                .AddClass("notifications").Id("notifications")
                .Add(T["Send Notification"], "1", client => client
                    .Action("Index", "SendNotifications", "OrchardCore.Notifications")
                    .LocalNav()
                )
                .Add(T["Notifcation Settings"], "2", client => client
                    .Action("Index", "NotificationSettings", "OrchardCore.Notifications")
                    .LocalNav()
                )
            );

            return Task.CompletedTask;
        }
    }
}
