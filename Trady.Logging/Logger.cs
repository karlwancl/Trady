using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Logging
{
    public static class Logger
    {
        private static ILogger _logger = new LoggerFactory().CreateLogger("Trady");

        public static void Debug(string message)
            => _logger.LogDebug(message);

        public static void Info(string message)
            => _logger.LogInformation(message);

        public static void Warn(string message)
            => _logger.LogWarning(message);

        public static void Error(Exception ex)
            => _logger.LogError(ex.ToString());
    }
}
