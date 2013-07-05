using System;
using Autofac;
using Microsoft.AspNet.SignalR.Hubs;

namespace AutofacExtensions.Integration.SignalR
{
	public class SignalRModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			if (builder == null) {
				throw new ArgumentNullException("builder");
			}
			builder.RegisterType<HubLifetimeScopeManager>().SingleInstance();
			builder.RegisterType<AutofacHubActivator>().As<IHubActivator>().SingleInstance();
		}
	}
}
