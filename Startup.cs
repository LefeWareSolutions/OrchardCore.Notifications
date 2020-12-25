using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;
using OrchardCore.Notifications.Controllers;
using OrchardCore.Security.Permissions;

namespace OrchardCore.Notifications
{
    public class Startup : StartupBase
    {
        private readonly AdminOptions _adminOptions;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IOptions<AdminOptions> adminOptions)
        {
            _configuration = configuration;
            _adminOptions = adminOptions.Value;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPermissionProvider, NotificationsPermissions>();
            services.AddScoped<INavigationProvider, NotificationsAdminMenu>();
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var notificationSettingsControllerName = typeof(NotificationSettingsController).ControllerName();
            routes.MapAreaControllerRoute(
                name: "NotificationSettings",
                areaName: "OrchardCore.Notifcations",
                pattern: _adminOptions.AdminUrlPrefix + "/Notifcations/Types/{NotificationId}/Instances/{action}",
                defaults: new { controller = notificationSettingsControllerName, action = "Index" }
            );

            var sendNotificationControllerName = typeof(SendNotificationsController).ControllerName();
            routes.MapAreaControllerRoute(
                name: "WorkflowTypes",
                areaName: "OrchardCore.Notifcations",
                pattern: _adminOptions.AdminUrlPrefix + "/Notifcations/Types/{action}/{id?}",
                defaults: new { controller = sendNotificationControllerName, action = "Index" }
            );
        }
    }

}
