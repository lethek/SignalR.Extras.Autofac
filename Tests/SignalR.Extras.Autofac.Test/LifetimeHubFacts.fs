namespace SignalR.Extras.Autofac.Test

module LifetimeHubFacts =
    open System
    open System.Linq
    open SignalR.Extras.Autofac
    open SignalR.Extras.Autofac.Test.Stubs
    open Xunit


    [<Fact>]
    let ``SignalR Resolves Valid Lifetime Hubs``() =
        let container = TestHelper.SetupAutofacContainer()
        let hubManager = TestHelper.SetupSignalRHubManager(container)

        use hub = hubManager.ResolveHub("DynamicLifetimeHubStub")
        Assert.IsType<DynamicLifetimeHubStub>(hub) |> ignore
        Assert.True(hub :? ILifetimeHub) |> ignore
        Assert.NotNull(hub)

        use hub = hubManager.ResolveHub("GenericLifetimeHubStub")
        Assert.IsType<GenericLifetimeHubStub>(hub) |> ignore
        Assert.True(hub :? ILifetimeHub) |> ignore
        Assert.NotNull(hub)


    [<Fact>]
    let ``SignalR Resolves Valid Ordinary Hubs``() =
        let container = TestHelper.SetupAutofacContainer()
        let hubManager = TestHelper.SetupSignalRHubManager(container)

        use hub = hubManager.ResolveHub("DynamicOrdinaryHubStub")
        Assert.IsType<DynamicOrdinaryHubStub>(hub) |> ignore
        Assert.False(hub :? ILifetimeHub);
        Assert.NotNull(hub)

        use hub = hubManager.ResolveHub("GenericOrdinaryHubStub")
        Assert.IsType<GenericOrdinaryHubStub>(hub) |> ignore
        Assert.False(hub :? ILifetimeHub)
        Assert.NotNull(hub)


    [<Fact>]
    let ``SignalR Resolves Correct Number of Registered Hubs``() =
        let container = TestHelper.SetupAutofacContainer()
        let hubManager = TestHelper.SetupSignalRHubManager(container)

        let hubs = hubManager.ResolveHubs()
        Assert.Equal(4, hubs.Count())


    [<Fact>]
    let ``SignalR Resolves New Hub Instance Each Time``() =
        let container = TestHelper.SetupAutofacContainer()
        let hubManager = TestHelper.SetupSignalRHubManager(container)

        use hub1 = hubManager.ResolveHub("DynamicLifetimeHubStub")
        use hub2 = hubManager.ResolveHub("DynamicLifetimeHubStub")
        Assert.NotSame(hub1, hub2)      

        use hub1 = hubManager.ResolveHub("GenericLifetimeHubStub")
        use hub2 = hubManager.ResolveHub("GenericLifetimeHubStub")
        Assert.NotSame(hub1, hub2);


    [<Fact>]
    let ``Receives New Instances of Scoped Dependencies``() =
        let container = TestHelper.SetupAutofacContainer()
        let hubManager = TestHelper.SetupSignalRHubManager(container)

        use hub1 = hubManager.ResolveHub("DynamicLifetimeHubStub") :?> DynamicLifetimeHubStub
        use hub2 = hubManager.ResolveHub("DynamicLifetimeHubStub") :?> DynamicLifetimeHubStub
        Assert.NotSame(hub1.ScopedDependency, hub2.ScopedDependency)
        Assert.Same(hub1.SingletonDependency, hub2.SingletonDependency)

        use hub1 = hubManager.ResolveHub("GenericLifetimeHubStub") :?> GenericLifetimeHubStub
        use hub2 = hubManager.ResolveHub("GenericLifetimeHubStub") :?> GenericLifetimeHubStub
        Assert.NotSame(hub1.ScopedDependency, hub2.ScopedDependency)
        Assert.Same(hub1.SingletonDependency, hub2.SingletonDependency)


    [<Fact>]
    let ``Dynamic Hub Owned Dependencies Dispose Correctly``() =
        let container = TestHelper.SetupAutofacContainer()
        let hubManager = TestHelper.SetupSignalRHubManager(container)

        let scopedDisposalCount = ref 0
        let singletonDisposalCount = ref 0

        (
            use hub = hubManager.ResolveHub("DynamicLifetimeHubStub") :?> DynamicLifetimeHubStub
            hub.ScopedDependency.OnDisposing.Add(fun() -> scopedDisposalCount := !scopedDisposalCount + 1)
            hub.SingletonDependency.OnDisposing.Add(fun() -> singletonDisposalCount := !singletonDisposalCount + 1)
        )

        //The singleton dependency doesn't belong to the hub's scope
        Assert.Equal(0, !singletonDisposalCount)

        //This dependency does and will be disposed when the hub is disposed
        Assert.Equal(1, !scopedDisposalCount)


    [<Fact>]
    let ``Generic Hub Owned Dependencies Dispose Correctly``() =
        let container = TestHelper.SetupAutofacContainer()
        let hubManager = TestHelper.SetupSignalRHubManager(container)

        let scopedDisposalCount = ref 0
        let singletonDisposalCount = ref 0

        (
            use hub = hubManager.ResolveHub("GenericLifetimeHubStub") :?> GenericLifetimeHubStub
            hub.ScopedDependency.OnDisposing.Add(fun() -> scopedDisposalCount := !scopedDisposalCount + 1)
            hub.SingletonDependency.OnDisposing.Add(fun() -> singletonDisposalCount := !singletonDisposalCount + 1)
        )

        //The singleton dependency doesn't belong to the hub's scope
        Assert.Equal(0, !singletonDisposalCount)

        //This dependency does and will be disposed when the hub is disposed
        Assert.Equal(1, !scopedDisposalCount)


    [<Fact>]
    let ``Unmanaged Lifetime Hubs Dispose Without Throwing``() =
        let container = TestHelper.SetupAutofacContainer()
        let hubManager = TestHelper.SetupSignalRHubManager(container)

        use dynamicHub = hubManager.ResolveHub("DynamicLifetimeHubStub") :?> DynamicLifetimeHubStub
        use genericHub = hubManager.ResolveHub("GenericLifetimeHubStub") :?> GenericLifetimeHubStub

        dynamicHub.Dispose()
        genericHub.Dispose()
