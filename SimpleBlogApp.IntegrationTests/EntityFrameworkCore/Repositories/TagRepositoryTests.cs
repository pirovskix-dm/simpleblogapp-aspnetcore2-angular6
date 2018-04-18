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
	public class TagRepositoryTests : RepositoryTests
	{
		private readonly ITagRepository repository;

		public TagRepositoryTests(ITestOutputHelper output) : base(output)
		{
			repository = new TagRepository(context);
		}

		[Fact]
		public async Task GetAllAsync_ValidRequest_ShouldBeReturned()
		{
			var tags = await CreateTagsInDBAsync() as IEnumerable<Tag>;

			var result = await repository.GetAllAsync(t => t);

			result.Should().NotBeNullOrEmpty();
			result.Count().Should().Be(tags.Count());
			result.Should().BeEquivalentTo(tags);
		}

		[Fact]
		public async Task GetAsync_ValidRequest_ShouldBeReturned()
		{
			var tag = await CreateTagInDBAsync();

			var result = await repository.GetAsync(tag.Id, t => t);

			result.Should().BeEquivalentTo(tag);
		}

		[Fact]
		public async Task GetAsync_NoCategoryWithGivenIdExists_ShouldNotBeReturned()
		{
			var tag = await CreateTagInDBAsync();

			var result = await repository.GetAsync(tag.Id + 1, t => t);

			result.Should().BeNull();
		}

		[Fact]
		public async Task Add_ValidRequest_ShouldBeAdded()
		{
			int newIdShouldBe = 1;
			var tagToAdd = new Tag()
			{
				Name = "Tag_1",
				IsActive = true,
				DateCreated = DateTime.Now,
				DateLastUpdated = DateTime.Now
			};

			repository.Add(tagToAdd);

			await context.SaveChangesAsync();
			context.Entry(tagToAdd).State = EntityState.Detached;
			var addedCategory = await context.Tags.SingleOrDefaultAsync(t => t.Id == newIdShouldBe);
			addedCategory.Should().NotBeNull();
			addedCategory.Id.Should().Be(newIdShouldBe);
			addedCategory.Should().BeEquivalentTo(tagToAdd);
		}

		[Fact]
		public async Task Remove_ValidRequest_ShouldBeRemoved()
		{
			var tagToRemove = await CreateTagInDBAsync();

			repository.Remove(new Tag() { Id = tagToRemove.Id });

			await context.SaveChangesAsync();
			var removedPost = await context.Posts.SingleOrDefaultAsync(p => p.Id == tagToRemove.Id);
			removedPost.Should().BeNull();
		}

		[Fact]
		public async Task Update_ValidRequest_ShouldBeUpdated()
		{
			var tagToUpdate = await CreateTagInDBAsync();
			var changedTag = new Tag()
			{
				Id = tagToUpdate.Id,
				Name = "New_Tag_Name",
				IsActive = false,
				DateCreated = DateTime.Now,
				DateLastUpdated = DateTime.Now
			};
			var updatedTagShouldBe = new Tag()
			{
				Id = changedTag.Id,
				Name = changedTag.Name,
				IsActive = true,
				DateCreated = tagToUpdate.DateCreated,
				DateLastUpdated = changedTag.DateLastUpdated
			};

			repository.Update(changedTag);

			await context.SaveChangesAsync();
			context.Entry(changedTag).State = EntityState.Detached;
			var updatedPost = await context.Tags.SingleOrDefaultAsync(p => p.Id == tagToUpdate.Id);
			updatedPost.Should().NotBeNull();
			updatedPost.Should().BeEquivalentTo(updatedTagShouldBe);
		}

		[Fact]
		public async Task FindByNameAsync_ValidRequest_ShouldBeReturned()
		{
			var numOfRecords = 5;
			var tagToRemove = await CreateTagsInDBAsync();

			var result = await repository.FindByNameAsync("Tag", numOfRecords, t => t);

			result.Should().NotBeNullOrEmpty();
			result.Count().Should().BeLessOrEqualTo(numOfRecords);
		}

		[Fact]
		public async Task FindByNamesAsync_ValidRequest_ShouldBeReturned()
		{
			var tagsToFine = new string[] { "Tag_1", "Tag_2" };
			var tagToRemove = await CreateTagsInDBAsync();

			var result = await repository.FindByNamesAsync(tagsToFine, t => t);

			result.Should().NotBeNullOrEmpty();
			result.Count().Should().Be(tagsToFine.Length);
		}

		private async Task<List<Tag>> CreateTagsInDBAsync()
		{
			var tags = Enumerable.Range(1, 10).Select(x => new Tag()
			{
				Id = x,
				Name = "Tag_" + x.ToString(),
				IsActive = true,
				DateCreated = DateTime.Now.AddDays(-x - 1),
				DateLastUpdated = DateTime.Now.AddDays(-x - 1)
			}).ToList();
			context.Tags.AddRange(tags);
			await context.SaveChangesAsync();
			foreach (var t in tags)
			{
				context.Entry(t).State = EntityState.Detached;
			}
			return tags;
		}

		private async Task<Tag> CreateTagInDBAsync()
		{
			var tag = new Tag()
			{
				Name = "Tag_1",
				IsActive = true,
				DateCreated = DateTime.Now,
				DateLastUpdated = DateTime.Now
			};
			context.Tags.Add(tag);
			await context.SaveChangesAsync();
			context.Entry(tag).State = EntityState.Detached;
			return tag;
		}
	}
}
