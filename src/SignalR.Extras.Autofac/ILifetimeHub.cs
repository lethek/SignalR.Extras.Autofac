using Microsoft.AspNet.SignalR.Hubs;


namespace SignalR.Extras.Autofac;

/// <summary>
/// Represents a hub with a lifetime management feature.
/// </summary>
public interface ILifetimeHub : IHub
{
    /// <summary>
    /// Occurs when the hub is being disposed.
    /// </summary>
    event EventHandler OnDisposing;
}
