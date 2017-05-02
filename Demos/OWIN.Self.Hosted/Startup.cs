using System;
using System.Reflection;

using Autofac;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;

using Owin;

using SignalR.Extras.Autofac;


namespace OWIN.Self.Hosted
{

	public class Startup
	{

		public void Configuration(IAppBuilder app)
		{
			var builder = new ContainerBuilder();
			builder.RegisterLifetimeHubManager();
			builder.RegisterHubs(Assembly.GetExecutingAssembly());
			builder.RegisterType<LogService>().AsImplementedInterfaces();
			Container = builder.Build();

			//Set SignalR's dependency resolver to be Autofac
			var hubConfig = new HubConfiguration {
				Resolver = new AutofacDependencyResolver(Container)
			};

			//Register the Autofac middleware FIRST, then the standard SignalR middleware.
			app.UseAutofacMiddleware(Container);
			app.MapSignalR("/signalr", hubConfig);
		}


		private static IContainer Container { get; set; }

	}

}
