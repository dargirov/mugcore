using MugStore.Data;
using MugStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace MugStore.Services.Common
{
    public class LoggerService : ILoggerService
    {
        private readonly IDbRepository<Log> logs;

        public LoggerService(IDbRepository<Log> logs)
        {
            this.logs = logs;
        }

        public void Log(LogLevel level, string message, string code = null, IPAddress ipAddress = null)
        {
            var log = new Log()
            {
                Code = code,
                Content = message,
                Level = level,
                IpAddress = ipAddress?.ToString()
            };

            this.logs.Add(log);
            this.logs.Save();
        }

        public IEnumerable<Log> GetLogMessages(Expression<Func<Log, bool>> predicate)
        {
            return this.logs.All().Where(predicate).OrderByDescending(x => x.Id).Take(100).ToList();
        }
    }
}
