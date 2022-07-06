namespace SignalR.Extras.Autofac.Tests.Stubs.Dependencies;

public abstract class ObjectStubBase : IDisposable
{

    public event EventHandler OnDisposing;

    public void Dispose()
    {
        var handler = OnDisposing;
        handler?.Invoke(this, EventArgs.Empty);
    }

}