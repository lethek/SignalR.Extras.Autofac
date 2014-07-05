using System;
using Microsoft.AspNet.SignalR;

namespace SignalR.Extras.Autofac
{
	/// <summary>
	/// Hubs with injected dependencies which must be scoped to the same lifetime should subclass
	/// LifetimeHub to enable proper and transparent lifetime-scope management.
	/// </summary>
	public abstract class LifetimeHub : Hub
	{
		internal event EventHandler Disposing;

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing) {
				var handler = Disposing;
				if (handler != null) {
					handler(this, EventArgs.Empty);
				}
			}
		}
	}
}
