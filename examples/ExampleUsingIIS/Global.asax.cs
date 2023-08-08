using System.Reflection;

using Autofac;
using Autofac.Integration.SignalR;

using ExampleUsingIIS.Hubs;
using ExampleUsingIIS.Services;

using Microsoft.AspNet.SignalR;

using SignalR.Extras.Autofac;


namespace ExampleUsingIIS
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
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

            //Build the container
            var container = builder.Build();

            //Set Autofac as SignalR's dependency resolver
            GlobalHost.DependencyResolver = new AutofacDependencyResolver(container);
        }
    }
}
