using MugStore.Data.Models;
using System.Collections.Generic;

namespace MugStore.Services.Common
{
    public interface ILoggerService
    {
        void Log(LogLevel level, string message, string code = null);
        IEnumerable<Log> GetLogMessages();
    }
}
