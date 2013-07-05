using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Microsoft.AspNet.SignalR.Hubs;

namespace AutofacExtensions.Integration.SignalR
{
	internal class HubLifetimeScopeManager : IDisposable
	{
		public T ResolveHub<T>(Type type, ILifetimeScope lifetimeScope) where T : LifetimeHub
		{
			var scope = lifetimeScope.BeginLifetimeScope();
			var hub = (T)scope.Resolve(type);
			hub.Disposing += HubOnDisposing;
			lock (_hubLifetimeScopes) {
				_hubLifetimeScopes.Add(hub, scope);
			}
			return hub;
		}

		public void Dispose()
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

		private void HubOnDisposing(object sender, EventArgs eventArgs)
		{
			ILifetimeScope scope = null;
			var hub = sender as IHub;
			lock (_hubLifetimeScopes) {
				if (hub != null && _hubLifetimeScopes.TryGetValue(hub, out scope)) {
					_hubLifetimeScopes.Remove(hub);
				}
			}
			if (scope != null) {
				scope.Dispose();
			}
		}

		private readonly Dictionary<IHub, ILifetimeScope> _hubLifetimeScopes = new Dictionary<IHub, ILifetimeScope>();
	}
}
