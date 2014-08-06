using System;
using NLog;

namespace MvcDemoSite.Models
{
    public class NLogger : ILog
    {
        private readonly Logger _logger;

        public NLogger(string logName)
        {
            _logger = LogManager.GetLogger(logName);
        }

        public void Trace(string message)
        {
            _logger.Trace(message);
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(message);
        }
    }
}