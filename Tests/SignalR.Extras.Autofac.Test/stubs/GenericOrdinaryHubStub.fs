namespace SignalR.Extras.Autofac.Test.Stubs

open Microsoft.AspNet.SignalR

type GenericOrdinaryHubStub() =
    inherit Hub<IHubClientStub>()
