using System.Reflection;

namespace AutofacExtensions.Integration.SignalR
{
	public abstract class AssemblyHook
	{
		public static Assembly Assembly { get { return typeof(AssemblyHook).Assembly; } }
	}
}
