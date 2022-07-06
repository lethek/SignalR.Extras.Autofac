namespace SignalR.Extras.Autofac;

public class ScopeLifetimeTag
{
    /// <summary>
    /// Tag used in setting up per-request lifetime scope registrations
    /// </summary>
    public static readonly object RequestLifetimeScopeTag = (object)"SignalRHub";
}