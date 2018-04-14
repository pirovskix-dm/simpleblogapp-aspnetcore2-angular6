using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace SimpleBlogApp.Extensions.Logger
{
	public class FileLogger : ILogger
	{
		private readonly string path;
		private object _lock = new object();

		public FileLogger(string path)
		{
			this.path = path;
			File.WriteAllText(path, "");
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return null;
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return true;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (formatter != null)
			{
				lock (_lock)
				{
					string[] filter = { "SELECT", "SET", "INSERT", "GET", "POST", "PUT", "DELETE" };
					string result = formatter(state, exception);
					if (filter.Any(f => result.Contains(f)))
						File.AppendAllText(path, result + Environment.NewLine + Environment.NewLine);
				}
			}
		}
	}
}
