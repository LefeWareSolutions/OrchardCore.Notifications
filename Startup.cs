using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;
using OrchardCore.Notifications.Controllers;
using OrchardCore.Notifications.SendNotifications.Models;
using OrchardCore.Security.Permissions;
using OrchardCore.Notifications.SendNotifications.Migrations;
using OrchardCore.Notifications.Handlers;

namespace OrchardCore.Notifications
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPermissionProvider, NotificationsPermissions>();
            services.AddScoped<INavigationProvider, NotificationsAdminMenu>();
        }
    }

    [Feature(NotificationsConstants.Features.NotificationSettings)]
    public class NotifcationSettingsStartup : StartupBase
    {
        private readonly AdminOptions _adminOptions;

        public NotifcationSettingsStartup(IOptions<AdminOptions> adminOptions)
        {
            _adminOptions = adminOptions.Value;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var notificationSettingsControllerName = typeof(NotificationSettingsController).ControllerName();
            routes.MapAreaControllerRoute(
                name: "NotificationSettings",
                areaName: "OrchardCore.Notifications",
                pattern: _adminOptions.AdminUrlPrefix + "/NotificationsSettings",
                defaults: new { controller = notificationSettingsControllerName, action = "Index" }
            );

            //routes.MapAreaControllerRoute(
            //    name: "NotificationSettings",
            //    areaName: "OrchardCore.Notifications",
            //    pattern: _adminOptions.AdminUrlPrefix + "/NotificationsSettings/Disable/",
            //    defaults: new { controller = notificationSettingsControllerName, action = "Index" }
            //);
        }

    }

    [Feature(NotificationsConstants.Features.SendNotifications)]
    public class SendNotifcationsStartup : StartupBase
    {
        private readonly AdminOptions _adminOptions;

        public SendNotifcationsStartup(IOptions<AdminOptions> adminOptions)
        {
            _adminOptions = adminOptions.Value;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            //ContentParts
            services.AddContentPart<Notification>()
                .AddHandler<NotificationHandler>();
            services.AddContentPart<NotificationEmailMessagePart>();
            services.AddContentPart<NotificationRecipientPart>();
            //Migrations
            services.AddScoped<IDataMigration, SendNotificationsMigrations>();
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            // var sendNotificationControllerName = typeof(SendNotificationsController).ControllerName();
            // routes.MapAreaControllerRoute(
            //     name: "SendNotificiation",
            //     areaName: "OrchardCore.Notifications",
            //     pattern: _adminOptions.AdminUrlPrefix + "/SendNotification",
            //     defaults: new { controller = sendNotificationControllerName, action = "Index" }
            // );
        }

    }

}
