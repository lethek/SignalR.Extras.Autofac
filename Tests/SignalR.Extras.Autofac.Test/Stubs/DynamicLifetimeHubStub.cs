using SignalR.Extras.Autofac.Test.Stubs.Dependencies;


namespace SignalR.Extras.Autofac.Test.Stubs
{

	public class DynamicLifetimeHubStub : LifetimeHub
	{

		public DynamicLifetimeHubStub(ScopedObjectStub dep1, SingletonObjectStub dep2, RequestScopedObjectStub dep3)
		{
			ScopedDependency = dep1;
			SingletonDependency = dep2;
			RequestScopedDependency = dep3;
		}

		public readonly ScopedObjectStub ScopedDependency;
		public readonly SingletonObjectStub SingletonDependency;
		public readonly RequestScopedObjectStub RequestScopedDependency;

	}

}