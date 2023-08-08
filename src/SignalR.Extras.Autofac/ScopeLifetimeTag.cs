namespace SignalR.Extras.Autofac;


/// <summary>
/// A class containing a tag used in setting up per-request lifetime scope registrations
/// </summary>
public static class ScopeLifetimeTag
{
    /// <summary>
    /// Tag used in setting up per-request lifetime scope registrations
    /// </summary>
    public static readonly object RequestLifetimeScopeTag = "SignalRHub";
}
