# OrchardCore.Notifications
The OrchardCore.Notifications project is an easy way to set up and send out notifications to both internal and external users. The project is made  made up of two features.

##Send Notifications
Send custom notifications to different groups contained in your application:
- Individual users
- User Groups 
- custom

##Notification Settings
An abstraction on top of the exsiting OrchardCore.Workflows project that allows a user to enable/disable workflows that contain the email task. Also allows a user to customize the email in the workflow



## Setting up your dev environment
1. **Prerequisites:** Make sure you have an up-to-date clone of [the Orchard Core repository](https://github.com/OrchardCMS/OrchardCore) on the `dev` branch. Please consult [the Orchard Core documentation](https://orchardcore.readthedocs.io/en/latest/) and make sure you have a working Orchard before you proceed. You'll also, of course, need all of Orchard Core's prerequisites for development (.NET Core, a code editor, etc.). The following steps assume some basic understanding of Orchard Core.
2. Clone the module under `[your Orchard Core clone's root]/src/OrchardCore.Modules`.
3. Add the existing project to the solution under `src/OrchardCore.Modules` in the solution explorer if you're using Visual Studio.
4. Add a reference to the module from the `OrchardCore.Cms.Web` project.
5. Build, run.
6. From the admin, enable the module's only feature.
