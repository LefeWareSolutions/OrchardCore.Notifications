using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Notifications.ViewModels;
using OrchardCore.Settings;
using OrchardCore.Workflows.Indexes;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using YesSql;

namespace OrchardCore.Notifications.Controllers
{
    [Admin]
    [Feature(NotificationsConstants.Features.NotificationSettings)]
    public class NotificationSettingsController : Controller
    {
        private readonly ISiteService _siteService;
        private readonly ISession _session;
        private readonly IWorkflowTypeStore _workflowTypeStore;
        private readonly IAuthorizationService _authorizationService;
        private readonly INotifier _notifier;
        private readonly IHtmlLocalizer H;

        public NotificationSettingsController(IAuthorizationService authorizationService, ISiteService siteService,
        ISession session, IWorkflowTypeStore workflowTypeStore, INotifier notifier, IHtmlLocalizer<NotificationSettingsController> h)
        {
            _authorizationService = authorizationService;
            _siteService = siteService;
            _session = session;
            _workflowTypeStore = workflowTypeStore;
            _notifier = notifier;
            H = h;
        }

        [HttpGet]
        public async Task<IActionResult> Index(PagerParameters pagerParameters)
        {
            if (!await _authorizationService.AuthorizeAsync(User, NotificationsPermissions.EditNotificationSettings))
            {
                return Forbid();
            }

            var siteSettings = await _siteService.GetSiteSettingsAsync();
            var pager = new Pager(pagerParameters, siteSettings.PageSize);

            //TODO: Need a way to distinguish notification specific workflows
            var query = _session.Query<WorkflowType, WorkflowTypeIndex>();
            var workflowTypes = await query
                .Skip(pager.GetStartIndex())
                .Take(pager.PageSize)
                .ListAsync();

            //var pagerShape = (await New.Pager(pager)).TotalItemCount(count).RouteData(routeData);
            var model = new NotificationSettingsViewModel
            {
                Notifications = workflowTypes.Select(x => new NotificationEntry
                {
                    Id = x.Id,
                    Enabled = x.IsEnabled,
                    Name = x.Name
                }).ToList(),
                //Pager = pagerShape
            };
            return View(model);
        }

        [HttpPut]
        public async Task<IActionResult> EnableNotifcation(string id)
        {
            if (!await _authorizationService.AuthorizeAsync(User, NotificationsPermissions.EditNotificationSettings))
            {
                return Forbid();
            }

            await SetNotifcationEnabled(id, true);

            _notifier.Success(H["Notifcation settings updated."]);
             return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditNotification(string id)
        {
            if (!await _authorizationService.AuthorizeAsync(User, NotificationsPermissions.EditNotificationSettings))
            {
                return Forbid();
            }
            var workflowType = await _workflowTypeStore.GetAsync(id);

             return RedirectToAction("Index");
        }

        [HttpPut]
        public async Task<IActionResult> DisableNotifcation(string id)
        {
            if (!await _authorizationService.AuthorizeAsync(User, NotificationsPermissions.EditNotificationSettings))
            {
                return Forbid();
            }

            await SetNotifcationEnabled(id, false);

            _notifier.Success(H["Notifcation settings updated."]);
             return RedirectToAction("Index");
        }

        private async Task SetNotifcationEnabled(string id, bool enabled)
        {
            var workflowType = await _workflowTypeStore.GetAsync(id);
            workflowType.IsEnabled = enabled;
            await _workflowTypeStore.SaveAsync(workflowType);
            await _session.CommitAsync();
        }
    }
}
