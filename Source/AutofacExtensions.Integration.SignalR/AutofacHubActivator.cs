using Autofac;
using Microsoft.AspNet.SignalR.Hubs;

namespace AutofacExtensions.Integration.SignalR
{
	internal class AutofacHubActivator : IHubActivator
	{
		public AutofacHubActivator(LifetimeHubManager lifetimeHubManager, ILifetimeScope lifetimeScope)
		{
			LifetimeScope = lifetimeScope;
			LifetimeHubManager = lifetimeHubManager;
		}

		public IHub Create(HubDescriptor descriptor)
		{
			//If requested type is a LifetimeHub, let the LifetimeHubManager nest a new lifetime-scope
			//before resolving and then hook up disposal notifications. Otherwise simply resolve and return.
			return typeof(LifetimeHub).IsAssignableFrom(descriptor.HubType)
				? LifetimeHubManager.ResolveHub<LifetimeHub>(descriptor.HubType, LifetimeScope)
				: LifetimeScope.Resolve(descriptor.HubType) as IHub;
		}

		private ILifetimeScope LifetimeScope { get; set; }
		private LifetimeHubManager LifetimeHubManager { get; set; }
	}
}
