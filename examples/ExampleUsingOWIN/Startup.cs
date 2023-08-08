using System.Reflection;

using Autofac;
using Autofac.Integration.SignalR;

using ExampleUsingOWIN.Hubs;
using ExampleUsingOWIN.Services;

using Microsoft.AspNet.SignalR;

using Owin;

using SignalR.Extras.Autofac;


namespace ExampleUsingOWIN
{

	public class Startup
	{

        /// <summary>
        /// <para>With the exception of registering the LifetimeHubManager, this is a standard startup class for configuring Autofac with
        /// SignalR in an OWIN application, as described in the Autofac documentation here:</para>
        /// <para><see cref="https://docs.autofac.org/en/latest/integration/signalr.html#owin-integration"/></para>
        /// </summary>
        /// <param name="app"></param>
		public void Configuration(IAppBuilder app)
		{
			var builder = new ContainerBuilder();

            //IMPORTANT: Register the LifetimeHubManager - this is required to support per-request hub dependencies
			builder.RegisterLifetimeHubManager();

            //Register the SignalR hubs, either by scanning assemblies...
            //builder.RegisterHubs(Assembly.GetExecutingAssembly());

            //Or registering individually (make sure they're all marked ExternallyOwned so SignalR can manage their disposal)
            builder.RegisterType<EchoHub>().ExternallyOwned();

            //Register your other services which need to be injected into your hubs
			builder.RegisterType<LogService>().AsImplementedInterfaces().InstancePerLifetimeScope();

			var container = builder.Build();

			//Register the Autofac middleware FIRST, then the standard SignalR middleware.
			app.UseAutofacMiddleware(container);
            app.MapSignalR("/signalr", new HubConfiguration {
                //Set Autofac as SignalR's dependency resolver
                Resolver = new AutofacDependencyResolver(container)
            });
        }

	}

}
