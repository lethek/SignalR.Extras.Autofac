using System.Reflection;

namespace SignalR.Extras.Autofac
{
	public abstract class AssemblyHook
	{
		public static Assembly Assembly { get { return typeof(AssemblyHook).Assembly; } }
	}
}
