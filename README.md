# SignalR.Extras.Autofac

[![Build & Publish](https://github.com/lethek/SignalR.Extras.Autofac/actions/workflows/dotnet.yml/badge.svg)](https://github.com/lethek/SignalR.Extras.Autofac/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/nuget/v/SignalR.Extras.Autofac.svg)](https://www.nuget.org/packages/SignalR.Extras.Autofac)
[![GitHub license](https://img.shields.io/github/license/lethek/SignalR.Extras.Autofac)](https://github.com/lethek/SignalR.Extras.Autofac/blob/master/LICENSE)

*This library is designed for use with SignalR 2.x in the .NET Framework.*

SignalR.Extras.Autofac directly addresses a current limitation of Autofac where it does not provide a mechanism to create a lifetime-scope per SignalR hub invocation. SignalR already creates a separate hub instance for each call, and Autofac is able to inject any dependencies, but neither library provides a way for those dependencies lives to be bound to the lifetime of the hub.

SignalR.Extras.Autofac provides a simple, transparent way to bridge that gap, effectively allowing per-request dependency injection for SignalR hubs.

## Usage:

1. Install the NuGet package: [SignalR.Extras.Autofac](https://www.nuget.org/packages/SignalR.Extras.Autofac)

2. Reference the namespace:
  ```csharp
  using SignalR.Extras.Autofac;
  ```

3. Call the new RegisterLifetimeHubManager extension method on your ContainerBuilder instance, e.g.:

  ```csharp
  builder.RegisterLifetimeHubManager();
  ```

4. When setting up an Autofac container in your project, follow the usual Autofac & SignalR integration steps as outlined on the Autofac wiki (https://autofac.readthedocs.io/en/v6.0.0/integration/signalr.html), i.e. replace SignalR's dependency resolver with Autofac's custom one and register your hubs as you normally would. If you're registering your hubs manually, you still need to configure the registrations with ExternallyOwned().

5. Ensure that your SignalR hubs which require automatic lifetime scope management (it'll be per hub-invocation) inherit from the `LifetimeHub` or `LifetimeHub<T>` classes.

Your hub instances will automatically and transparently be assigned their own new child lifetime scopes upon each invocation by SignalR. They will also automatically dispose of those lifetime scopes upon completion. You do not need to manage the disposal of those hubs which inherit from `LifetimeHub` or `LifetimeHub<T>`, or the injected dependencies owned by Autofac.

You can still register and use Hubs which do not inherit from `LifetimeHub` or `LifetimeHub<T>` - dependencies will still be injected correctly by Autofac, however you will have to manually manage their lifetime scopes yourself (as described here https://autofac.readthedocs.org/en/v6.0.0/integration/signalr.html#managing-dependency-lifetimes).

Note: disposing the Autofac container will result in any tracked LifetimeHub instances and their scoped dependencies also being disposed at that time.

## Examples:

### Registration on an IIS host

```csharp
// Create the container builder.
var builder = new ContainerBuilder();

// Register the LifetimeHub manager.
builder.RegisterLifetimeHubManager();

// Register the SignalR hubs.
builder.RegisterHubs(Assembly.GetExecutingAssembly()); //Register all hubs in an assembly

// Register other dependencies.
builder.Register(c => new UnitOfWork()).As<IUnitOfWork>().InstancePerLifetimeScope();

// Build the container.
var container = builder.Build();

// Configure SignalR with an instance of the Autofac dependency resolver.
GlobalHost.DependencyResolver = new AutofacDependencyResolver(container);
```

### Registration on an OWIN host

```csharp
// Create the container builder.
var builder = new ContainerBuilder();

// Register the LifetimeHub manager.
builder.RegisterLifetimeHubManager();

// Register the SignalR hubs.
builder.RegisterHubs(Assembly.GetExecutingAssembly()); //Register all hubs in an assembly

// Register other dependencies.
builder.Register(c => new UnitOfWork()).As<IUnitOfWork>().InstancePerLifetimeScope();

// Build the container.
var container = builder.Build();

// Register the Autofac middleware FIRST, then the standard SignalR middleware.
app.UseAutofacMiddleware(Container);

// Configure the standard SignalR middleware and ensure the Autofac dependency resolver is used.
app.MapSignalR("/signalr", new HubConfiguration {
  Resolver = new AutofacDependencyResolver(Container)
});
```

### Your hubs

```csharp
public class MyHub : LifetimeHub
{
    public MyHub(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }

    public void DoSomething() {
        //The hub instance and dependencies like UnitOfWork are automatically created prior to SignalR invoking this method
        //Do stuff here
        //The hub instance and dependencies like UnitOfWork are automatically destroyed after SignalR has invoked this method
    }

    private IUnitOfWork _unitOfWork;
}

public class SomeHub : LifetimeHub<ISomeClient>
{
    public SomeHub(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }

    public void DoSomething() {
        //The hub instance and dependencies like UnitOfWork are automatically created prior to SignalR invoking this method
        //Do stuff here
        //The hub instance and dependencies like UnitOfWork are automatically destroyed after SignalR has invoked this method
    }

    private IUnitOfWork _unitOfWork;
}
```
