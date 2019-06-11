namespace Serko.Common.Log
{
    public interface ILogger
    {
        void LogInformation<T>(string log);
        void LogWarning<T>(string log);
        void LogError<T>(string log);
        void Flush();
    }
}
