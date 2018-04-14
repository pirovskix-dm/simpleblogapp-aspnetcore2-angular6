using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Core.Query;
using SimpleBlogApp.Core.Query.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SimpleBlogApp.Core.Interfaces
{
	public interface IPostRepository : IRepository<Post>
	{
		Task<QueryResult<T>> GetQueryResultAsync<T>(PostQuery queryObj, Expression<Func<Post, T>> exp);
		Task<IEnumerable<IdObject<T>>> GetTagsAsync<T>(Expression<Func<IdObject<Tag>, IdObject<T>>> exp);
		Task<IEnumerable<IdObject<T>>> GetTagsAsync<T>(int post, Expression<Func<IdObject<Tag>, IdObject<T>>> exp);
		Task<IEnumerable<IdObject<T>>> GetTagsAsync<T>(IEnumerable<int> posts, Expression<Func<IdObject<Tag>, IdObject<T>>> exp);
	}
}