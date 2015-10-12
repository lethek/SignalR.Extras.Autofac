namespace SignalR.Extras.Autofac.Test

module LifetimeHubManagerFacts =
    open System
    open System.Linq
    open Autofac
    open SignalR.Extras.Autofac
    open Xunit


    [<Fact>]
    let ``On Registration Throws If Container Builder Is Null``() =
        let builder : ContainerBuilder = null
        Assert.Throws<ArgumentNullException>(fun () -> builder.RegisterLifetimeHubManager())


    [<Fact>]
    let ``Autofac Can Resolve LifetimeHubManager``() =
        let container = TestHelper.SetupAutofacContainer()
        let lifetimeHubManager = container.Resolve<LifetimeHubManager>()
        Assert.NotNull(lifetimeHubManager)


    [<Fact>]
    let ``OnDisposing Disposes Owned Hubs``() =
        let container = TestHelper.SetupAutofacContainer()
        let hubManager = TestHelper.SetupSignalRHubManager(container)

        let disposedHubsCount = ref 0
        let hubs = hubManager.ResolveHubs().OfType<ILifetimeHub>()
        for hub in hubs do
            hub.OnDisposing.Add(fun(x) -> disposedHubsCount := !disposedHubsCount + 1)
        let lifetimeHubManager = container.Resolve<LifetimeHubManager>()
        lifetimeHubManager.Dispose()
        Assert.Equal(2, !disposedHubsCount)
