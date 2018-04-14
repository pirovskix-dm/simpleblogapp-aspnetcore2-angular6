using System.Collections.Generic;

namespace SimpleBlogApp.Core.Query
{
	public class QueryResult<T>
	{
		public int TotalItems { get; set; }
		public IEnumerable<T> Items { get; set; }
	}
}
