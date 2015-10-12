namespace SignalR.Extras.Autofac.Test.Stubs

open SignalR.Extras.Autofac
open SignalR.Extras.Autofac.Test.Stubs.Dependencies

type GenericLifetimeHubStub(dep1 : ScopedObjectStub, dep2 : SingletonObjectStub) =
    inherit LifetimeHub<IHubClientStub>()
    member this.ScopedDependency = dep1
    member this.SingletonDependency = dep2
