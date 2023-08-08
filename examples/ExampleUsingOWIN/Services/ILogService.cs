namespace ExampleUsingOWIN.Services
{
    public interface ILogService
    {
        void Debug(string msg, params object[] args);
    }
}
