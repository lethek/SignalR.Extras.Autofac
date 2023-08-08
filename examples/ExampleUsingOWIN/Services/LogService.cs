using System;

namespace ExampleUsingOWIN.Services
{
    public class LogService : ILogService
    {
	    public void Debug(string msg, params object[] args)
			=> Console.WriteLine(msg, args);
    }
}
