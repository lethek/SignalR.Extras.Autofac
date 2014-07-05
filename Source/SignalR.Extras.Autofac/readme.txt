SignalR.Hubs.Autofac

Extends integration between Autofac and SignalR to enable transparent lifetime scope management for hubs.

Usage:

1. Install the NuGet package: https://www.nuget.org/packages/SignalR.Hubs.Autofac

2. Reference the namespace: SignalR.Hubs.Autofac

3. When setting up an Autofac container in your project, call the new RegisterLifetimeHubManager extension method on your ContainerBuilder instance, e.g.:

  builder.RegisterLifetimeHubManager();

4. Ensure that your SignalR hubs inherit the LifetimeHub class
