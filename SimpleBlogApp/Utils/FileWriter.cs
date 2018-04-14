using System;
using System.IO;

namespace SimpleBlogApp.Utils
{
	public static class FileWriter
	{
		private static string path;

		static FileWriter()
		{
			path = @"D:\MyProjsss\SimpleBlogApp\SimpleBlogApp\logger.txt";

			if (!File.Exists(path))
				throw new Exception(path + " path does not exists");
		}

		public static void WriteLine(string text)
		{
			WriteMessage(text);
		}

		public static void WriteKeyValueLine(string key, string value)
		{
			WriteMessage(key + ": >" + value + "<");
		}

		private static void WriteMessage(string message)
		{
			File.AppendAllText(path, "####### Custom Message ===> " + message + Environment.NewLine + Environment.NewLine);
		}
	}
}
