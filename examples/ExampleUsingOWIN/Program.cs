using System;

using Microsoft.Owin.Hosting;


namespace ExampleUsingOWIN
{
	public class Program
	{
		static void Main(string[] args)
		{
			using (WebApp.Start<Startup>("http://localhost:8080/")) {
				Console.WriteLine("Server running at http://localhost:8080/");
                Console.WriteLine();
                Console.WriteLine("Press ENTER to exit...");
				Console.ReadLine();
			}
		}
	}
}
