using Autofac;
using Microsoft.AspNet.SignalR.Hubs;

namespace AutofacExtensions.Integration.SignalR
{
	/// <summary>
	/// SignalR HubActivator for creating hub instances with the correct lifetime-scope for automatic per-instance session & transaction management.
	/// </summary>
	internal class AutofacHubActivator : IHubActivator
	{
		public AutofacHubActivator(HubLifetimeScopeManager hubLifetimeScopeManager, ILifetimeScope lifetimeScope)
		{
			LifetimeScope = lifetimeScope;
			HubLifetimeScopeManager = hubLifetimeScopeManager;
		}

		public IHub Create(HubDescriptor descriptor)
		{
			return typeof(LifetimeHub).IsAssignableFrom(descriptor.HubType)
				? HubLifetimeScopeManager.ResolveHub<LifetimeHub>(descriptor.HubType, LifetimeScope)
				: LifetimeScope.Resolve(descriptor.HubType) as IHub;
		}

		private ILifetimeScope LifetimeScope { get; set; }
		private HubLifetimeScopeManager HubLifetimeScopeManager { get; set; }
	}
}
