using MugStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;

namespace MugStore.Services.Common
{
    public interface ILoggerService
    {
        void Log(LogLevel level, Exception ex, string code = null, IPAddress ipAddress = null);
        IEnumerable<Log> GetLogMessages(Expression<Func<Log, bool>> predicate);
    }
}
