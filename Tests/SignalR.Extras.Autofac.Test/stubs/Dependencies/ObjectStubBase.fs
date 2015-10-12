namespace SignalR.Extras.Autofac.Test.Stubs.Dependencies

open System

[<AbstractClass>]
type ObjectStubBase() =
    let onDisposing = Event<_>()
    member this.OnDisposing = onDisposing.Publish

    interface IDisposable with
        member this.Dispose() = onDisposing.Trigger()
