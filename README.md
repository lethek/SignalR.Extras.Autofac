# AutofacExtensions
=================

## Usage:

1. Install the NuGet package: https://www.nuget.org/packages/AutofacExtensions.Integration.SignalR

2. Reference the namespace: AutofacExtensions.Integration.SignalR

3. When setting up an Autofac container in your project, call the new RegisterLifetimeHubManager extension method on your ContainerBuilder instance, e.g.:

```C#
builder.RegisterLifetimeHubManager();
```

4. Ensure that your SignalR hubs inherit the LifetimeHub class
