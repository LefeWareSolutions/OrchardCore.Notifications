using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace OrchardCore.Notifications.SendNotifications.Migrations
{
    public class SendNotificationsMigrations: DataMigration
    {
        IContentDefinitionManager _contentDefinitionManager;

        public SendNotificationsMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition("Notification", part => part
                .Attachable()
                .WithDescription("Default Fields for notifcations")
            );

            _contentDefinitionManager.AlterPartDefinition("NotificationRecipientPart", part => part
                .Attachable()
                .WithDescription("Fields for notifcation recipients")
                .WithField("UserEmails", field => field
                    .WithDisplayName("User Emails")
                    .OfType("UserPickerField")
                    .WithSettings(new UserPickerFieldSettings() { Hint = "Add user email addresses", Multiple = true })
                )
                .WithField("Emails", field => field
                    .WithDisplayName("Add Additional Emails")
                    .OfType("TextField")
                    .WithSettings(new TextFieldSettings(){Hint = "Add email addresses seperated by comas" })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("NotificationEmailMessagePart", part => part
                .Attachable()
                .WithDescription("Fields for notifcations")
                .WithField("From", field => field
                    .OfType("TextField")
                    .WithDisplayName("From")
                )
                .WithField("Cc", field => field
                    .OfType("TextField")
                    .WithDisplayName("Cc")
                )
                .WithField("Bcc", field => field
                    .OfType("TextField")
                    .WithDisplayName("Bcc")
                )
                .WithField("Subject", field => field
                    .OfType("TextField")
                    .WithDisplayName("Subject")
                    .WithSettings(new TextFieldSettings(){Required = true })
                )
                .WithField("MessageBody", field => field
                    .OfType("HtmlField")
                    .WithDisplayName("Body")
                    .WithEditor("Wysiwyg")
                    .WithSettings(new TextFieldSettings(){Required = true })
                )
            );


            _contentDefinitionManager.AlterTypeDefinition("Notification", builder => builder
                .Creatable()
                .Listable()
                .WithPart("Notification", part=>part.WithPosition("1"))
                .WithPart("NotificationRecipientPart", part=>part.WithPosition("2"))
                .WithPart("NotificationEmailMessagePart", part => part.WithPosition("3"))
            );

            return 1;
        }
    }
}
