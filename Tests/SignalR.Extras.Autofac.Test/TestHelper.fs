namespace SignalR.Extras.Autofac.Test

open Autofac
open Autofac.Integration.SignalR
open Microsoft.AspNet.SignalR.Hubs
open SignalR.Extras.Autofac
open SignalR.Extras.Autofac.Test.Stubs
open SignalR.Extras.Autofac.Test.Stubs.Dependencies

[<AbstractClass; Sealed>]
type internal TestHelper private() =
    static member SetupAutofacContainer() =
        let builder = ContainerBuilder()
        builder.RegisterLifetimeHubManager()
        //Register stub dependencies for the hubs
        builder.RegisterType<ScopedObjectStub>().InstancePerLifetimeScope() |> ignore
        builder.RegisterType<SingletonObjectStub>().SingleInstance() |> ignore
        //Register our LifetimeHubs
        builder.RegisterType<DynamicLifetimeHubStub>().ExternallyOwned() |> ignore
        builder.RegisterType<GenericLifetimeHubStub>().ExternallyOwned() |> ignore
        //Register ordinary, untracked SignalR hubs
        builder.RegisterType<DynamicOrdinaryHubStub>().ExternallyOwned() |> ignore
        builder.RegisterType<GenericOrdinaryHubStub>().ExternallyOwned() |> ignore
        builder.Build()
    static member SetupSignalRHubManager(container: IContainer) =
        //Configure SignalR to use Autofac for dependency resolution
        let resolver = new AutofacDependencyResolver(container)
        //Note: SignalR's DefaultHubManager below is unrelated to our LifetimeHubManager
        DefaultHubManager(resolver)
