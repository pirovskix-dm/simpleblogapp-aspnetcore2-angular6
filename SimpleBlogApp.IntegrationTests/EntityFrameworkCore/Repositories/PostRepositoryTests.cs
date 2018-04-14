using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SimpleBlogApp.Core.Interfaces;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Core.Query;
using SimpleBlogApp.Core.Query.Models;
using SimpleBlogApp.EntityFrameworkCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SimpleBlogApp.IntegrationTests.EntityFrameworkCore.Repositories
{
	public class PostRepositoryTests : RepositoryTests
	{
		private readonly IPostRepository repository;

		public PostRepositoryTests(ITestOutputHelper output) : base(output)
		{
			repository = new PostRepository(context);
		}

		[Fact]
		public async Task GetAllAsync_ValidRequest_ShouldBeReturned()
		{
			var posts = await CreatePostsInDBAsync() as IEnumerable<Post>;

			var result = await repository.GetAllAsync(p => p);

			result.Should().NotBeNullOrEmpty();
			result.Count().Should().Be(posts.Count());
			result.Should().BeEquivalentTo(posts, opt => opt.Excluding(p => p.Category));
		}

		[Fact]
		public async Task GetAsync_ValidRequest_ShouldBeReturned()
		{
			var post = await CreatePostInDBAsync();

			var result = await repository.GetAsync(post.Id, p => p);

			result.Should().NotBeNull();
			result.Should().BeEquivalentTo(post, opt => opt.Excluding(p => p.Category));
		}

		[Fact]
		public async Task GetAsync_NoPostWithGivenIdExists_ShouldNotBeReturned()
		{
			var post = await CreatePostInDBAsync();

			var result = await repository.GetAsync(post.Id + 1, p => p);

			result.Should().BeNull();
		}

		[Fact]
		public async Task Add_ValidRequest_ShouldBeAdded()
		{
			int newIdShouldBe = 1;
			var postToAdd = new Post()
			{
				Title = "Title_1",
				ShortContent = "ShortContent_1",
				Content = "Content_1",
				CategoryId = 1,
				IsActive = true
			};

			repository.Add(postToAdd);

			await context.SaveChangesAsync();
			context.Entry(postToAdd).State = EntityState.Detached;
			var addedPost = await context.Posts.SingleOrDefaultAsync(p => p.Id == newIdShouldBe);
			addedPost.Should().NotBeNull();
			postToAdd.Id.Should().Be(newIdShouldBe);
			addedPost.Should().BeEquivalentTo(postToAdd, opt => opt.Excluding(p => p.Category));
		}

		[Fact]
		public async Task Remove_ValidRequest_ShouldBeRemoved()
		{
			var postToRemove = await CreatePostInDBAsync();

			repository.Remove(new Post() { Id = postToRemove.Id });

			await context.SaveChangesAsync();
			var removedPost = await context.Posts.SingleOrDefaultAsync(p => p.Id == postToRemove.Id);
			removedPost.Should().BeNull();
		}

		[Fact]
		public async Task Update_ValidRequest_ShouldBeUpdated()
		{
			var postToUpdate = await CreatePostInDBAsync();
			var changedPost = new Post()
			{
				Id = postToUpdate.Id,
				Title = "New_title",
				ShortContent = "New_ShortContent",
				Content = "New_Content",
				CategoryId = 2,
				IsActive = false,
				DateCreated = DateTime.Now,
				DateLastUpdated = DateTime.Now
			};
			var updatedPostShouldBe = new Post()
			{
				Id = postToUpdate.Id,
				Title = changedPost.Title,
				ShortContent = changedPost.ShortContent,
				Content = changedPost.Content,
				CategoryId = changedPost.CategoryId,
				IsActive = true,
				DateCreated = postToUpdate.DateCreated,
				DateLastUpdated = changedPost.DateLastUpdated
			};

			repository.Update(changedPost);

			await context.SaveChangesAsync();
			context.Entry(changedPost).State = EntityState.Detached;
			var updatedPost = await context.Posts.SingleOrDefaultAsync(p => p.Id == postToUpdate.Id);
			updatedPost.Should().NotBeNull();
			updatedPost.Should().BeEquivalentTo(updatedPostShouldBe, opt => opt.Excluding(p => p.Category));
		}

		[Fact]
		public async Task GetQueryResultAsyncWithPaging_ValidRequest_ShouldBeReturned()
		{
			var posts = await CreatePostsInDBAsync();
			var queryObj = new PostQuery()
			{
				CategoryId = 2,
				SortBy = "Id",
				IsSortAscending = false,
				Page = 2,
				PageSize = 3
			};
			var queryResultShouldBe = new QueryResult<Post>()
			{
				TotalItems = posts.Where(p => p.CategoryId == queryObj.CategoryId).Count(),
				Items = posts
					.Where(p => p.CategoryId == queryObj.CategoryId)
					.OrderByDescending(p => p.Id)
					.Skip(3).Take(3)
			};

			var result = await repository.GetQueryResultAsync(queryObj, p => p);

			result.Should().NotBeNull();
			result.TotalItems.Should().Be(queryResultShouldBe.TotalItems);
			result.Items.Should().NotBeNullOrEmpty();
			result.Items.Count().Should().Be(queryResultShouldBe.Items.Count());
			result.Items.Should().BeEquivalentTo(queryResultShouldBe.Items, opt => opt.Excluding(p => p.Category));
		}

		private async Task<List<Post>> CreatePostsInDBAsync()
		{
			await CreateCategoriesInDBAsync();
			var posts = Enumerable.Range(1, 20).Select(x => new Post()
			{
				Id = x,
				Title = "Title_" + x.ToString(),
				ShortContent = "ShortContent_" + x.ToString(),
				Content = "Content_" + x.ToString(),
				CategoryId = x < 10 ? 1 : 2,
				IsActive = true
			}).ToList();
			context.Posts.AddRange(posts);
			await context.SaveChangesAsync();
			foreach (var p in posts)
				context.Entry(p).State = EntityState.Detached;
			return posts;
		}

		private async Task<Post> CreatePostInDBAsync()
		{
			await CreateCategoriesInDBAsync();
			var post = new Post()
			{
				Title = "Title_1",
				ShortContent = "ShortContent_1",
				Content = "Content_1",
				CategoryId = 1,
				IsActive = true
			};
			context.Posts.Add(post);
			await context.SaveChangesAsync();
			context.Entry(post).State = EntityState.Detached;
			return post;
		}

		private async Task<List<Category>> CreateCategoriesInDBAsync()
		{
			var categories = Enumerable.Range(1, 4).Select(x => new Category() { Id = x }).ToList();
			context.Categories.AddRange(categories);
			await context.SaveChangesAsync();
			foreach (var c in categories)
				context.Entry(c).State = EntityState.Detached;
			return categories;
		}
	}
}
