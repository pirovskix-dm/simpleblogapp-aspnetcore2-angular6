using SimpleBlogApp.Core.Interfaces;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Core.Query.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SimpleBlogApp.Extensions
{
	public static class IQueryableExtensions
	{
		public static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> query, IQueryObject queryObj) 
			where TEntity : class
		{
			query.NotNull();
			queryObj.NotNull();

			if (queryObj.Page <= 0)
				queryObj.Page = 1;

			if (queryObj.PageSize <= 0)
				queryObj.PageSize = 10;

			query = queryObj.Page == 1 ? query : query.Skip((queryObj.Page - 1) * queryObj.PageSize);
			return query.Take(queryObj.PageSize);
		}

		public static IQueryable<TEntity> ApplyOrdering<TEntity>(this IQueryable<TEntity> query, IQueryObject queryObj) 
			where TEntity : class
		{
			query.NotNull();

			if (string.IsNullOrWhiteSpace(queryObj.SortBy))
				return query;

			return query.OrderBy(queryObj.SortBy, queryObj.IsSortAscending);
		}

		public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query, string propertyString, bool IsSortAscending)
			where TEntity : class
		{
			query.NotNull();

			if (string.IsNullOrWhiteSpace(propertyString))
				return (IOrderedQueryable<TEntity>)query;

			string[] propSequence = propertyString.Split('.', StringSplitOptions.RemoveEmptyEntries);
			var parameter = Expression.Parameter(typeof(TEntity), "x");
			Expression property = Expression.Property(parameter, propSequence[0]);

			for (int i = 1; i < propSequence.Length; i++)
				property = Expression.Property(property, propSequence[i]);

			var lambda = Expression.Lambda(property, parameter);

			var methodName = IsSortAscending ? "OrderBy" : "OrderByDescending";
			var orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == methodName && x.GetParameters().Length == 2);
			var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TEntity), property.Type);
			var result = orderByGeneric.Invoke(null, new object[] { query, lambda });

			return (IOrderedQueryable<TEntity>)result;
		}

		public static IQueryable<Post> ApplyFiltering(this IQueryable<Post> query, PostQuery queryObj)
		{
			query.NotNull();
			queryObj.NotNull();

			if (queryObj.CategoryId.HasValue)
				query = query.Where(p => p.CategoryId == queryObj.CategoryId.Value);

			if (queryObj.TagId.HasValue)
				query = query.Where(p => p.Tags.Any(pt => pt.TagId == queryObj.TagId.Value));

			return query;
		}

		public static IQueryable<Post> ApplySearching(this IQueryable<Post> query, PostQuery queryObj)
		{
			query.NotNull();

			if (string.IsNullOrWhiteSpace(queryObj.Search))
				return query;

			return query.Where(p =>
				p.Title.Contains(queryObj.Search) ||
				p.Content.Contains(queryObj.Search) ||
				p.ShortContent.Contains(queryObj.Search) ||
				p.Category.Name.Contains(queryObj.Search)
			);
		}
	}
}