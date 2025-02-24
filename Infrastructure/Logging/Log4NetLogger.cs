using log4net;
using System;

namespace NotifyHub.Infrastructure.Logging
{
    public class Log4NetLogger : ILoggerService
    {
        private readonly ILog _logger;

        public Log4NetLogger()
        {
            _logger = LogManager.GetLogger(typeof(Log4NetLogger));
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogWarning(string message)
        {
            _logger.Warn(message);
        }

        public void LogError(string message, Exception ex = null)
        {
            if (ex != null)
                _logger.Error(message, ex);
            else
                _logger.Error(message);
        }
    }
}