namespace SignalR.Extras.Autofac.Test.Stubs

open SignalR.Extras.Autofac
open SignalR.Extras.Autofac.Test.Stubs.Dependencies

type DynamicLifetimeHubStub(dep1 : ScopedObjectStub, dep2 : SingletonObjectStub) =
    inherit LifetimeHub()
    member this.ScopedDependency = dep1
    member this.SingletonDependency = dep2
