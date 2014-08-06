using System;

namespace MvcDemoSite.Models
{
    public interface ILog
    {
        void Trace(string message);
        void Error(string message, Exception exception);
    }
}
