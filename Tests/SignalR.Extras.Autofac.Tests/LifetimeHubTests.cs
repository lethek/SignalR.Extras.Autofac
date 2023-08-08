﻿using SignalR.Extras.Autofac.Tests.Stubs;

using static SignalR.Extras.Autofac.Tests.TestHelper;


namespace SignalR.Extras.Autofac.Tests;

public class LifetimeHubTests
{

    [Fact]
    public void SignalRResolvesValidLifetimeHubs()
    {
        var container = SetupAutofacContainer();
        var hubManager = SetupSignalRHubManager(container);

        using (var hub = hubManager.ResolveHub(nameof(DynamicLifetimeHubStub))) {
            Assert.IsType<DynamicLifetimeHubStub>(hub);
            Assert.True(hub is ILifetimeHub);
            Assert.NotNull(hub);
        }

        using (var hub = hubManager.ResolveHub(nameof(GenericLifetimeHubStub))) {
            Assert.IsType<GenericLifetimeHubStub>(hub);
            Assert.True(hub is ILifetimeHub);
            Assert.NotNull(hub);
        }
    }


    [Fact]
    public void SignalRResolvesValidOrdinaryHubs()
    {
        var container = SetupAutofacContainer();
        var hubManager = SetupSignalRHubManager(container);

        using (var hub = hubManager.ResolveHub(nameof(DynamicOrdinaryHubStub))) {
            Assert.IsType<DynamicOrdinaryHubStub>(hub);
            Assert.False(hub is ILifetimeHub);
            Assert.NotNull(hub);
        }

        using (var hub = hubManager.ResolveHub(nameof(GenericOrdinaryHubStub))) {
            Assert.IsType<GenericOrdinaryHubStub>(hub);
            Assert.False(hub is ILifetimeHub);
            Assert.NotNull(hub);
        }
    }


    [Fact]
    public void SignalRResolvesCorrectNumberOfRegisteredHubs()
    {
        var container = SetupAutofacContainer();
        var hubManager = SetupSignalRHubManager(container);

        var hubs = hubManager.ResolveHubs();
        Assert.Equal(4, hubs.Count());
    }


    [Fact]
    public void SignalRResolvesNewHubInstanceEachTime()
    {
        var container = SetupAutofacContainer();
        var hubManager = SetupSignalRHubManager(container);

        using (var hub1 = (ILifetimeHub)hubManager.ResolveHub(nameof(DynamicLifetimeHubStub)))
        using (var hub2 = (ILifetimeHub)hubManager.ResolveHub(nameof(DynamicLifetimeHubStub))) {
            Assert.NotSame(hub1, hub2);
        }

        using (var hub1 = (ILifetimeHub)hubManager.ResolveHub(nameof(GenericLifetimeHubStub)))
        using (var hub2 = (ILifetimeHub)hubManager.ResolveHub(nameof(GenericLifetimeHubStub))) {
            Assert.NotSame(hub1, hub2);
        }
    }


    [Fact]
    public void ReceivesNewInstancesOfScopedDependencies()
    {
        var container = SetupAutofacContainer();
        var hubManager = SetupSignalRHubManager(container);

        using (var hub1 = (DynamicLifetimeHubStub)hubManager.ResolveHub(nameof(DynamicLifetimeHubStub)))
        using (var hub2 = (DynamicLifetimeHubStub)hubManager.ResolveHub(nameof(DynamicLifetimeHubStub))) {
            Assert.NotSame(hub1.ScopedDependency, hub2.ScopedDependency);
            Assert.Same(hub1.SingletonDependency, hub2.SingletonDependency);
            Assert.NotSame(hub1.RequestScopedDependency, hub2.RequestScopedDependency);
        }

        using (var hub1 = (GenericLifetimeHubStub)hubManager.ResolveHub(nameof(GenericLifetimeHubStub)))
        using (var hub2 = (GenericLifetimeHubStub)hubManager.ResolveHub(nameof(GenericLifetimeHubStub))) {
            Assert.NotSame(hub1.ScopedDependency, hub2.ScopedDependency);
            Assert.Same(hub1.SingletonDependency, hub2.SingletonDependency);
            Assert.NotSame(hub1.RequestScopedDependency, hub2.RequestScopedDependency);
        }
    }


    [Fact]
    public void DynamicHubOwnedDependenciesDisposeCorrectly()
    {
        var container = SetupAutofacContainer();
        var hubManager = SetupSignalRHubManager(container);

        int scopedDisposalCount = 0;
        int singletonDisposalCount = 0;
        int requestScopedDisposalCount = 0;
        using (var hub = (DynamicLifetimeHubStub)hubManager.ResolveHub(nameof(DynamicLifetimeHubStub))) {
            hub.ScopedDependency.OnDisposing += (s, a) => scopedDisposalCount++;
            hub.SingletonDependency.OnDisposing += (s, a) => singletonDisposalCount++;
            hub.RequestScopedDependency.OnDisposing += (s, a) => requestScopedDisposalCount++;
        }

        //The singleton dependency doesn't belong to the hub's scope
        Assert.Equal(0, singletonDisposalCount);

        //These dependencies belong to the hub's scope and will be disposed when the hub is disposed
        Assert.Equal(1, scopedDisposalCount);
        Assert.Equal(1, requestScopedDisposalCount);
    }


    [Fact]
    public void GenericHubOwnedDependenciesDisposeCorrectly()
    {
        var container = SetupAutofacContainer();
        var hubManager = SetupSignalRHubManager(container);

        int scopedDisposalCount = 0;
        int singletonDisposalCount = 0;
        int requestScopedDisposalCount = 0;
        using (var hub = (GenericLifetimeHubStub)hubManager.ResolveHub(nameof(GenericLifetimeHubStub))) {
            hub.ScopedDependency.OnDisposing += (s, a) => scopedDisposalCount++;
            hub.SingletonDependency.OnDisposing += (s, a) => singletonDisposalCount++;
            hub.RequestScopedDependency.OnDisposing += (s, a) => requestScopedDisposalCount++;
        }

        //The singleton dependency doesn't belong to the hub's scope
        Assert.Equal(0, singletonDisposalCount);

        //These dependencies belong to the hub's scope and will be disposed when the hub is disposed
        Assert.Equal(1, scopedDisposalCount);
        Assert.Equal(1, requestScopedDisposalCount);
    }


    [Fact]
    public void UnmanagedLifetimeHubsDisposeWithoutThrowing()
    {
        new DynamicLifetimeHubStub(null, null, null).Dispose();
        new GenericLifetimeHubStub(null, null, null).Dispose();
    }

}