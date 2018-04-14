using SimpleBlogApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SimpleBlogApp.Core.Interfaces
{
	public interface ITagRepository : IRepository<Tag>
	{
		Task<IEnumerable<T>> FindByNameAsync<T>(string name, int numOfRecords, Expression<Func<Tag, T>> exp);
		Task<IEnumerable<T>> FindByNamesAsync<T>(IEnumerable<string> names, Expression<Func<Tag, T>> exp);
		void AddRange(IEnumerable<Tag> tags);
	}
}
