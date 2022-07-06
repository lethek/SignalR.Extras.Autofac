using Microsoft.AspNet.SignalR.Hubs;


namespace SignalR.Extras.Autofac;

public interface ILifetimeHub : IHub
{
    event EventHandler OnDisposing;
}