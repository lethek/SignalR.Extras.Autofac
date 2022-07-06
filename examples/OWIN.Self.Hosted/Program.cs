using System;

using Microsoft.Owin.Hosting;


namespace OWIN.Self.Hosted
{
	class Program
	{
		static void Main(string[] args)
		{
			using (WebApp.Start<Startup>("http://localhost:8080/")) {
				Console.WriteLine("Server running at http://localhost:8080/");
				Console.ReadLine();
			}
		}
	}
}
