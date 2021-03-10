using System;

namespace GithubActions.Shared.Logging
{
    public enum LogLevel
    {
        None = -1,
        All = 0,
        Info = 1,
        Error = 2
    }

    public interface ISimpleLogger
    {
        void Info(string output);

        void Error(string output);
    }

    public class SimpleLogger : ISimpleLogger
    {
        private readonly LogLevel _logLevel;

        public SimpleLogger(LogLevel logLevel = LogLevel.All)
        {
            _logLevel = logLevel;
        }

        public void Info(string output)
        {
            if (_logLevel == LogLevel.All || _logLevel == LogLevel.Info)
            {
                Console.WriteLine(output);
            }
        }

        public void Error(string output)
        {
            if (_logLevel == LogLevel.All || _logLevel == LogLevel.Error)
            {
                Console.WriteLine(output);
            }
        }
    }
}