using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;

namespace OrchardCore.Notifications
{
    public class NotificationsMigrations : DataMigration
    {
        IContentDefinitionManager _contentDefinitionManager;

        public NotificationsMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            return 1;
        }
    }
}
