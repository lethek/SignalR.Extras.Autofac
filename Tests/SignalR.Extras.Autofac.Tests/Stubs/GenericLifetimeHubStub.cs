using SignalR.Extras.Autofac.Tests.Stubs.Dependencies;

namespace SignalR.Extras.Autofac.Tests.Stubs;

public class GenericLifetimeHubStub : LifetimeHub<IHubClientStub>
{

    public GenericLifetimeHubStub(ScopedObjectStub dep1, SingletonObjectStub dep2, RequestScopedObjectStub dep3)
    {
        ScopedDependency = dep1;
        SingletonDependency = dep2;
        RequestScopedDependency = dep3;
    }

    public readonly ScopedObjectStub ScopedDependency;
    public readonly SingletonObjectStub SingletonDependency;
    public readonly RequestScopedObjectStub RequestScopedDependency;

}