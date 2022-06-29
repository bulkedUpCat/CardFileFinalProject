namespace CardFileApi.Logging
{
    /// <summary>
    /// Interface to be implemented by classes to log messages
    /// </summary>
    public interface ILoggerManager
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogDebug(string message);
        void LogError(string message);
    }
}
