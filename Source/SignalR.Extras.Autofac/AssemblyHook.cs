using System.Diagnostics.CodeAnalysis;
using System.Reflection;


namespace SignalR.Extras.Autofac
{
	[ExcludeFromCodeCoverage]
	public abstract class AssemblyHook
	{
		public static Assembly Assembly => typeof(AssemblyHook).Assembly;
	}
}
