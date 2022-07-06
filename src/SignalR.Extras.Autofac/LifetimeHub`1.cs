using Microsoft.AspNet.SignalR;


namespace SignalR.Extras.Autofac;

/// <summary>
/// Hubs with injected dependencies which must be scoped to the same lifetime should derive
/// from LifetimeHub&lt;T&gt; to enable proper and transparent lifetime-scope management.
/// </summary>
public abstract class LifetimeHub<T> : Hub<T>, ILifetimeHub
    where T : class
{
    public event EventHandler? OnDisposing;

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing) {
            OnDisposing?.Invoke(this, EventArgs.Empty);
        }
    }
}