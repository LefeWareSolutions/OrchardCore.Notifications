using OrchardCore.Modules.Manifest;
using OrchardCore.Notifications;

[assembly: Module(
    Name = "Notifications",
    Author = "LefeWare Solutions",
    Website = "https://LefeWareSolutions.com",
    Version = "1.0.0",
    Category = "Notifications"
)]

[assembly: Feature(
    Id = NotificationsConstants.Features.SendNotifications,
    Name = "Send Notifications",
    Description = "Send Notifications to users and user groups",
    Category = "LefeWare Solutions",
    Dependencies = new string[] { "OrchardCore.UserGroups"}
)]

[assembly: Feature(
    Id = NotificationsConstants.Features.NotificationSettings,
    Name = "Notification Settings",
    Category = "LefeWare Solutions",
    Description = "Add the ability to turn on/off OrchardCore Event Notifications",
    Dependencies = new string[] { }
)]
