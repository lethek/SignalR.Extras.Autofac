using Autofac;
using Autofac.Integration.SignalR;

using Microsoft.AspNet.SignalR.Hubs;

using SignalR.Extras.Autofac.Tests.Stubs;
using SignalR.Extras.Autofac.Tests.Stubs.Dependencies;


namespace SignalR.Extras.Autofac.Tests;

internal static class TestHelper
{

    internal static IContainer SetupAutofacContainer()
    {
        var builder = new ContainerBuilder();
        builder.RegisterLifetimeHubManager();

        //Register stub dependencies for the hubs
        builder.RegisterType<ScopedObjectStub>().InstancePerLifetimeScope();
        builder.RegisterType<SingletonObjectStub>().SingleInstance();
        builder.RegisterType<RequestScopedObjectStub>().InstancePerRequest(ScopeLifetimeTag.RequestLifetimeScopeTag);

        //Register our LifetimeHubs
        builder.RegisterType<DynamicLifetimeHubStub>().ExternallyOwned();
        builder.RegisterType<GenericLifetimeHubStub>().ExternallyOwned();

        //Register ordinary, untracked SignalR hubs  
        builder.RegisterType<DynamicOrdinaryHubStub>().ExternallyOwned();
        builder.RegisterType<GenericOrdinaryHubStub>().ExternallyOwned();

        return builder.Build();
    }


    internal static IHubManager SetupSignalRHubManager(IContainer container)
    {
        //Configure SignalR to use Autofac for dependency resolution
        var resolver = new AutofacDependencyResolver(container);

        //Note: SignalR's DefaultHubManager below is unrelated to our LifetimeHubManager
        return new DefaultHubManager(resolver);
    }

}