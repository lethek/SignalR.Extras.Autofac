using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;

using Microsoft.AspNet.SignalR.Hubs;


namespace SignalR.Extras.Autofac
{

	internal class LifetimeHubManager : IDisposable
	{

		public T ResolveHub<T>(Type type, ILifetimeScope lifetimeScope)
			where T : ILifetimeHub
		{
			var scope = lifetimeScope.BeginLifetimeScope();
			var hub = (T)scope.Resolve(type);
			hub.OnDisposing += HubOnDisposing;
			lock (_hubLifetimeScopes) {
				_hubLifetimeScopes.Add(hub, scope);
			}
			return hub;
		}


		/// <summary>
		/// If the LifetimeHubManager is disposed, make sure that all lifetime-scopes that it's
		/// still tracking also get disposed.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}


		protected virtual void Dispose(bool disposing)
		{
			ILifetimeScope[] scopes;
			lock (_hubLifetimeScopes) {
				scopes = _hubLifetimeScopes.Select(x => x.Value).ToArray();
				_hubLifetimeScopes.Clear();
			}
			foreach (var scope in scopes) {
				scope.Dispose();
			}
		}


		/// <summary>
		/// Make sure that the lifetime-scope associated with the hub is disposed
		/// </summary>
		private void HubOnDisposing(object sender, EventArgs eventArgs)
		{
			ILifetimeScope scope = null;
			var hub = sender as IHub;
			lock (_hubLifetimeScopes) {
				if (hub != null && _hubLifetimeScopes.TryGetValue(hub, out scope)) {
					_hubLifetimeScopes.Remove(hub);
				}
			}
			scope?.Dispose();
		}


		private readonly Dictionary<IHub, ILifetimeScope> _hubLifetimeScopes = new Dictionary<IHub, ILifetimeScope>();
	}

}
