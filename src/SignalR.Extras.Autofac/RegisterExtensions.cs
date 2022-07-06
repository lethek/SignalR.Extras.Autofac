using Autofac;

using Microsoft.AspNet.SignalR.Hubs;


namespace SignalR.Extras.Autofac;

public static class RegisterExtensions
{
    public static void RegisterLifetimeHubManager(this ContainerBuilder builder)
    {
        if (builder == null) {
            throw new ArgumentNullException(nameof(builder));
        }
        builder.RegisterType<LifetimeHubManager>().SingleInstance();
        builder.RegisterType<AutofacHubActivator>().As<IHubActivator>().SingleInstance();
    }
}