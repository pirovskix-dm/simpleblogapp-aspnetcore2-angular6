using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SimpleBlogApp.Core.Interfaces;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.EntityFrameworkCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SimpleBlogApp.IntegrationTests.EntityFrameworkCore.Repositories
{
	public class CategoryRepositoryTests : RepositoryTests
	{
		private readonly ICategoryRepository repository;

		public CategoryRepositoryTests(ITestOutputHelper output) : base(output)
		{
			repository = new CategoryRepository(context);
		}

		[Fact]
		public async Task GetAllAsync_ValidRequest_ShouldBeReturned()
		{
			var categories = await CreateCategoriesInDBAsync() as IEnumerable<Category>;

			var result = await repository.GetAllAsync(c => c);

			result.Should().NotBeNullOrEmpty();
			result.Count().Should().Be(categories.Count());
			result.Should().BeEquivalentTo(categories);
		}

		[Fact]
		public async Task GetAsync_ValidRequest_ShouldBeReturned()
		{
			var category = await CreateCategoryInDBAsync();

			var result = await repository.GetAsync(category.Id, c => c);

			result.Should().BeEquivalentTo(category);
		}

		[Fact]
		public async Task GetAsync_NoCategoryWithGivenIdExists_ShouldNotBeReturned()
		{
			var category = await CreateCategoryInDBAsync();

			var result = await repository.GetAsync(category.Id + 1, c => c);

			result.Should().BeNull();
		}

		[Fact]
		public async Task Add_ValidRequest_ShouldBeAdded()
		{
			int newIdShouldBe = 1;
			var categoryToAdd = new Category()
			{
				Name = "Category_1",
				IsActive = true,
				DateCreated = DateTime.Now,
				DateLastUpdated = DateTime.Now
			};

			repository.Add(categoryToAdd);

			await context.SaveChangesAsync();
			context.Entry(categoryToAdd).State = EntityState.Detached;
			var addedCategory = await context.Categories.SingleOrDefaultAsync(c => c.Id == newIdShouldBe);
			addedCategory.Should().NotBeNull();
			addedCategory.Id.Should().Be(newIdShouldBe);
			addedCategory.Should().BeEquivalentTo(categoryToAdd);
		}

		[Fact]
		public async Task Remove_ValidRequest_ShouldBeRemoved()
		{
			var categoryToRemove = await CreateCategoryInDBAsync();

			repository.Remove(new Category() { Id = categoryToRemove.Id });

			await context.SaveChangesAsync();
			var removedPost = await context.Posts.SingleOrDefaultAsync(p => p.Id == categoryToRemove.Id);
			removedPost.Should().BeNull();
		}

		[Fact]
		public async Task Update_ValidRequest_ShouldBeUpdated()
		{
			var categoryToUpdate = await CreateCategoryInDBAsync();
			var changedCategory = new Category()
			{
				Id = categoryToUpdate.Id,
				Name = "New_Category_Name",
				IsActive = false,
				DateCreated = DateTime.Now,
				DateLastUpdated = DateTime.Now
			};
			var updatedCategoryShouldBe = new Category()
			{
				Id = changedCategory.Id,
				Name = changedCategory.Name,
				IsActive = true,
				DateCreated = categoryToUpdate.DateCreated,
				DateLastUpdated = changedCategory.DateLastUpdated
			};

			repository.Update(changedCategory);

			await context.SaveChangesAsync();
			context.Entry(changedCategory).State = EntityState.Detached;
			var updatedPost = await context.Categories.SingleOrDefaultAsync(p => p.Id == categoryToUpdate.Id);
			updatedPost.Should().NotBeNull();
			updatedPost.Should().BeEquivalentTo(updatedCategoryShouldBe);
		}

		[Fact]
		public async Task IsExistAsync_ValidRequest_ShouldReturnTrue()
		{
			string name = "Category 1";
			var categoryToRemove = await CreateCategoryInDBAsync(name);

			bool result = await repository.IsExistAsync(name);

			result.Should().BeTrue();
		}

		[Fact]
		public async Task IsExistAsync_NoCategoryWithGivenNameExists_ShouldReturnTrue()
		{
			string name = "Category 1";
			var categoryToRemove = await CreateCategoryInDBAsync(name);

			bool result = await repository.IsExistAsync("Category 2");

			result.Should().BeFalse();
		}

		private async Task<Category> CreateCategoryInDBAsync(string name = "Category name")
		{
			var dateCreated = DateTime.Now;
			var category = new Category()
			{
				Name = name,
				IsActive = true,
				DateCreated = dateCreated,
				DateLastUpdated = dateCreated
			};
			context.Categories.Add(category);
			await context.SaveChangesAsync();
			context.Entry(category).State = EntityState.Detached;
			return category;
		}

		private async Task<List<Category>> CreateCategoriesInDBAsync()
		{
			var categories = Enumerable.Range(1, 7).Select(x => new Category()
			{
				Id = x,
				Name = "Category_" + x.ToString(),
				IsActive = true,
				DateCreated = DateTime.Now.AddDays(-x - 1),
				DateLastUpdated = DateTime.Now.AddDays(-x - 1)
			}).ToList();
			context.Categories.AddRange(categories);
			await context.SaveChangesAsync();
			foreach (var c in categories)
			{
				context.Entry(c).State = EntityState.Detached;
			}
			return categories;
		}
	}
}
