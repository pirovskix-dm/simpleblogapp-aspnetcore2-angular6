using Microsoft.EntityFrameworkCore;
using SimpleBlogApp.Core.Interfaces;
using SimpleBlogApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SimpleBlogApp.EntityFrameworkCore.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly SimpleBlogAppDbContext context;

		public CategoryRepository(SimpleBlogAppDbContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<Category, T>> exp)
		{
			return await context.Categories
				.OrderBy(c => c.Name)
				.Select(exp)
				.ToListAsync();
		}

		public async Task<T> GetAsync<T>(int id, Expression<Func<Category, T>> exp)
		{
			return await context.Categories
				.Where(c => c.Id == id)
				.Select(exp)
				.SingleOrDefaultAsync();
		}

		public void Add(Category category)
		{
			context.Categories.Add(category);
		}

		public void Remove(Category category)
		{
			context.Categories.Remove(category);
		}

		public void Update(Category category)
		{
			var entry = context.Categories.Update(category);
			entry.Property(e => e.IsActive).IsModified = false;
			entry.Property(e => e.DateCreated).IsModified = false;
		}

		public async Task<bool> IsExistAsync(string name)
		{
			return await context.Categories.AnyAsync(t => t.Name == name);
		}
	}
}
