﻿using Microsoft.Extensions.Logging;

namespace SimpleBlogApp.Extensions.Logger
{
	public class FileLoggerProvider : ILoggerProvider
	{
		private readonly string path;

		public FileLoggerProvider(string path)
		{
			this.path = path;
		}

		public ILogger CreateLogger(string categoryName)
		{
			return new FileLogger(path);
		}

		public void Dispose()
		{
			
		}
	}
}
