using System;
using Microsoft.AspNet.SignalR;

namespace AutofacExtensions.Integration.SignalR
{
	/// <summary>
	/// Hubs that have injected dependencies which must be scoped to the same lifetime should
	/// inherit this class to enable automatic lifetime scope management and prevent memory leaks.
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
