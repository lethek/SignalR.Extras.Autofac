using Microsoft.AspNet.SignalR;


namespace SignalR.Extras.Autofac;

/// <summary>
/// Base class for a SignalR hub that supports Per-Request lifetime dependencies.
/// Injected dependencies will be resolved automatically from a new nested lifetime scope on each call,
/// and the nested lifetime scope will be disposed when the hub is disposed upon completion of each call.
/// </summary>
public abstract class LifetimeHub : Hub, ILifetimeHub
{
    /// <summary>
    /// Event that is triggered when the hub is being disposed.
    /// </summary>
    public event EventHandler? OnDisposing;

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing) {
            OnDisposing?.Invoke(this, EventArgs.Empty);
        }
    }
}
