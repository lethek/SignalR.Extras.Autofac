using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SignalR.Extras.Autofac;

namespace OWIN.Self.Hosted
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
