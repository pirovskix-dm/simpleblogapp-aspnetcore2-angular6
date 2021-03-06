﻿using Microsoft.EntityFrameworkCore;
using SimpleBlogApp.Core.Interfaces;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SimpleBlogApp.EntityFrameworkCore.Repositories
{
	public class TagRepository : ITagRepository
	{
		private readonly SimpleBlogAppDbContext context;

		public TagRepository(SimpleBlogAppDbContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<Tag, T>> exp)
		{
			exp.NotNull();
			return await context.Tags.Select(exp).ToListAsync();
		}

		public async Task<T> GetAsync<T>(int id, Expression<Func<Tag, T>> exp)
		{
			exp.NotNull();
			return await context.Tags.Where(c => c.Id == id).Select(exp).SingleOrDefaultAsync();
		}

		public void Add(Tag tag)
		{
			tag.NotNull();
			context.Tags.Add(tag);
		}

		public void Remove(Tag tag)
		{
			tag.NotNull();
			context.Tags.Remove(tag);
		}

		public void Update(Tag tag)
		{
			tag.NotNull();
			var entry = context.Tags.Update(tag);
			entry.Property(e => e.IsActive).IsModified = false;
			entry.Property(e => e.DateCreated).IsModified = false;
		}

		public async Task<IEnumerable<T>> FindByNameAsync<T>(string name, int numOfRecords, Expression<Func<Tag, T>> exp)
		{
			name.NotNull();
			exp.NotNull();

			var query = context.Tags
				.Where(t => t.Name.Contains(name))
				.OrderBy(t => t.Name)
				.AsQueryable();

			if (numOfRecords > 0)
				query = query.Take(numOfRecords);

			return await query
				.Select(exp)
				.ToListAsync();
		}

		public async Task<IEnumerable<T>> FindByNamesAsync<T>(IEnumerable<string> names, Expression<Func<Tag, T>> exp)
		{
			names.NotNull();
			exp.NotNull();

			return await context.Tags
				.Where(t => names.Contains(t.Name))
				.Select(exp)
				.ToListAsync();
		}

		public void AddRange(IEnumerable<Tag> tags)
		{
			tags.NotNull();
			context.Tags.AddRange(tags);
		}
	}
}
