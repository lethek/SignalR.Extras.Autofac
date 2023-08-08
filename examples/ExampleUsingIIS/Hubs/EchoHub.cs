using ExampleUsingIIS.Services;

using SignalR.Extras.Autofac;


namespace ExampleUsingIIS.Hubs
{
    public class EchoHub : LifetimeHub
    {
	    public EchoHub(ILogService logService)
	    {
		    Log = logService;
	    }

	    public void Broadcast(string msg)
	    {
			Log.Debug("Broadcasting message: {0}", msg);
		    Clients.All.ReceiveMessage(msg);
		}

		private ILogService Log { get; }
	}
}
