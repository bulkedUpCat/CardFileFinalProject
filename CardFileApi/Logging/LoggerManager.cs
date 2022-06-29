using NLog;
using ILogger = NLog.ILogger;

namespace CardFileApi.Logging
{
    /// <summary>
    /// Custom logger class to log messages
    /// </summary>
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public LoggerManager()
        {

        }

        /// <summary>
        /// Logs DEBUG message to console
        /// </summary>
        /// <param name="message">Message to log</param>
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        /// <summary>
        /// Logs ERROR message to console
        /// </summary>
        /// <param name="message">Message to log</param>
        public void LogError(string message)
        {
            logger.Error(message);
        }

        /// <summary>
        /// Logs INFO message to console
        /// </summary>
        /// <param name="message">Message to log</param>
        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// Logs WARNING message to console
        /// </summary>
        /// <param name="message">Message to log</param>
        public void LogWarning(string message)
        {
            logger.Warn(message);
        }
    }
}
