using Autofac;

using Microsoft.AspNet.SignalR.Hubs;


namespace SignalR.Extras.Autofac;

internal class AutofacHubActivator : IHubActivator
{

    public AutofacHubActivator(LifetimeHubManager lifetimeHubManager, ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
        _lifetimeHubManager = lifetimeHubManager;
    }


    public IHub? Create(HubDescriptor descriptor)
        //If requested type is an ILifetimeHub, let the LifetimeHubManager nest a new lifetime-scope
        //before resolving and then hook up disposal notifications. Otherwise simply resolve and return.
        => typeof(ILifetimeHub).IsAssignableFrom(descriptor.HubType)
            ? _lifetimeHubManager.ResolveHub<ILifetimeHub>(descriptor.HubType, _lifetimeScope)
            : _lifetimeScope.Resolve(descriptor.HubType) as IHub;


    private readonly ILifetimeScope _lifetimeScope;
    private readonly LifetimeHubManager _lifetimeHubManager;

}