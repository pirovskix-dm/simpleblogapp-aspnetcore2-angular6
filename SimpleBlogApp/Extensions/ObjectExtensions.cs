using System;
using System.Collections.Generic;

namespace SimpleBlogApp.Extensions
{
	public static class ObjectExtensions
	{
		public static void NotNull(this object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("Value cannot be null.");
			}
		}

		public static void NotNull(this string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("Value cannot be null.");
			}
		}

		public static void NotNull<T>(this IEnumerable<T> coll)
		{
			if (coll == null)
			{
				throw new ArgumentNullException("Value cannot be null.");
			}
			else if (typeof(T).IsClass)
			{
				foreach(var c in coll)
				{
					if (c == null)
						throw new ArgumentNullException("Value cannot be null.");
				}
			}
		}
	}
}
