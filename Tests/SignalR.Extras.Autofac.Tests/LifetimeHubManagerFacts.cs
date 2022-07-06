using Autofac;

using static SignalR.Extras.Autofac.Tests.TestHelper;


namespace SignalR.Extras.Autofac.Tests;

public class LifetimeHubManagerFacts
{

    [Fact]
    public void OnRegistrationThrowsIfContainerBuilderIsNull()
    {
        ContainerBuilder builder = null;
        Assert.Throws<ArgumentNullException>(() => builder.RegisterLifetimeHubManager());
    }


    [Fact]
    public void AutofacCanResolveLifetimeHubManager()
    {
        var container = SetupAutofacContainer();

        var lifetimeHubManager = container.Resolve<LifetimeHubManager>();
        Assert.NotNull(lifetimeHubManager);
    }


    [Fact]
    public void OnDisposingDisposesOwnedHubs()
    {
        var container = SetupAutofacContainer();
        var hubManager = SetupSignalRHubManager(container);

        int disposedHubsCount = 0;
        var hubs = hubManager.ResolveHubs().OfType<ILifetimeHub>();
        foreach (var hub in hubs) {
            hub.OnDisposing += (o, a) => disposedHubsCount++;
        }
        var lifetimeHubManager = container.Resolve<LifetimeHubManager>();
        lifetimeHubManager.Dispose();
        Assert.Equal(2, disposedHubsCount);
    }

}