using MugStore.Data.Models;
using System.Collections.Generic;
using System.Net;

namespace MugStore.Services.Common
{
    public interface ILoggerService
    {
        void Log(LogLevel level, string message, string code = null, IPAddress ipAddress = null);
        IEnumerable<Log> GetLogMessages();
    }
}
