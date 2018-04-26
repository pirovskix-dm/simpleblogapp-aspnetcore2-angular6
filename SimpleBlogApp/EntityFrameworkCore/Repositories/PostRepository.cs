using Microsoft.EntityFrameworkCore;
using SimpleBlogApp.Core.Interfaces;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Core.Query;
using SimpleBlogApp.Core.Query.Models;
using SimpleBlogApp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SimpleBlogApp.EntityFrameworkCore.Repositories
{
	public class PostRepository : IPostRepository
	{
		private readonly SimpleBlogAppDbContext context;

		public PostRepository(SimpleBlogAppDbContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<Post, T>> exp)
		{
			exp.NotNull();
			return await context.Posts
				.Include(p => p.Category)
				.Include(p => p.Tags)
					.ThenInclude(pt => pt.Tag)
				.Select(exp)
				.ToListAsync();
		}

		public async Task<IEnumerable<IdObject<T>>> GetTagsAsync<T>(Expression<Func<IdObject<Tag>, IdObject<T>>> exp)
		{
			exp.NotNull();
			return await GetTags(null, exp).ToListAsync();
		}

		public async Task<IEnumerable<IdObject<T>>> GetTagsAsync<T>(int post, Expression<Func<IdObject<Tag>, IdObject<T>>> exp)
		{
			exp.NotNull();
			return await GetTags(new[] { post }, exp).ToListAsync();
		}

		public async Task<IEnumerable<IdObject<T>>> GetTagsAsync<T>(IEnumerable<int> posts, Expression<Func<IdObject<Tag>, IdObject<T>>> exp)
		{
			exp.NotNull();
			posts.NotNull();
			return await GetTags(posts, exp).ToListAsync();
		}

		private IQueryable<IdObject<T>> GetTags<T>(IEnumerable<int> posts, Expression<Func<IdObject<Tag>, IdObject<T>>> exp)
		{
			exp.NotNull();
			var query = context.PostTags
				.Include(pt => pt.Tag)
				.AsQueryable();

			if (posts != null && posts.Count() > 0)
				query = query.Where(pt => posts.Contains(pt.PostId));

			return query
				.Select(pt => new IdObject<Tag>() { Id = pt.PostId, Object = pt.Tag })
				.Select(exp);
		}

		public async Task<QueryResult<T>> GetQueryResultAsync<T>(PostQuery queryObj, Expression<Func<Post, T>> exp)
		{
			queryObj.NotNull();
			exp.NotNull();

			var result = new QueryResult<T>();

			var query = context.Posts
				.Include(p => p.Category)
				.Include(p => p.Tags)
					.ThenInclude(pt => pt.Tag)
				.AsQueryable();

			query = query
				.ApplyFiltering(queryObj)
				.ApplySearching(queryObj);

			result.TotalItems = await query.CountAsync();

			query = query
				.ApplyOrdering(queryObj)
				.ApplyPaging(queryObj);

			result.Items = await query
				.Select(exp)
				.ToListAsync();

			return result;
		}

		public async Task<T> GetAsync<T>(int id, Expression<Func<Post, T>> exp)
		{
			exp.NotNull();
			return await context.Posts
				.Include(p => p.Category)
				.Include(p => p.Tags)
					.ThenInclude(pt => pt.Tag)
				.Where(p => p.Id == id)
				.Select(exp)
				.SingleOrDefaultAsync();
		}

		public void Add(Post post)
		{
			post.NotNull();
			context.Posts.Add(post);
		}

		public void Remove(Post post)
		{
			post.NotNull();
			context.Posts.Remove(post);
		}

		public void Update(Post post)
		{
			post.NotNull();
			var entry = context.Posts.Update(post);
			entry.Property(e => e.IsActive).IsModified = false;
			entry.Property(e => e.DateCreated).IsModified = false;
		}
	}
}