using Autofac;

using Microsoft.AspNet.SignalR.Hubs;


namespace SignalR.Extras.Autofac;

/// <summary>
/// Extension methods for registering the LifetimeHubManager.
/// </summary>
public static class RegisterExtensions
{
    /// <summary>
    /// Registers the LifetimeHubManager with Autofac.
    /// </summary>
    /// <param name="builder">The ContainerBuilder to register with.</param>
    public static void RegisterLifetimeHubManager(this ContainerBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.RegisterType<LifetimeHubManager>().SingleInstance();
        builder.RegisterType<AutofacHubActivator>().As<IHubActivator>().SingleInstance();
    }
}
