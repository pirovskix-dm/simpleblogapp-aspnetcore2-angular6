using AutoMapper;
using FluentAssertions;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Mapping;
using SimpleBlogApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using Xunit;

namespace SimpleBlogApp.IntegrationTests.Mapping
{
	public class MappingProfileTests : IDisposable
	{
		public MappingProfileTests()
		{
			Mapper.Initialize(m => m.AddProfile<MappingProfile>());
			Mapper.AssertConfigurationIsValid();
		}

		public void Dispose()
		{
			Mapper.Reset();
		}

		[Fact]
		public void Map_Post_PostViewModel()
		{
			var post = new Post()
			{
				Id = 1,
				Title = "Title_1",
				ShortContent = "ShortContent_1",
				Content = "Content_1",
				CategoryId = 1,
				IsActive = true,
				Category = new Category()
				{
					Id = 1,
					Name = "Category_Name"
				},
				Tags = new List<PostTag>()
				{
					new PostTag() { PostId = 1, TagId = 1, Tag = new Tag { Id = 1, Name = "TagName1" } },
					new PostTag() { PostId = 1, TagId = 2, Tag = new Tag { Id = 2, Name = "TagName2" } },
					new PostTag() { PostId = 1, TagId = 3, Tag = new Tag { Id = 3, Name = "TagName3" } }
				},
				DateCreated = DateTime.Now,
				DateLastUpdated = DateTime.Now
			};
			var postViewModelShouldBe = new PostViewModel()
			{
				Id = post.Id,
				Title = post.Title,
				Content = post.Content,
				ShortContent = post.ShortContent,
				DateCreated = post.DateCreated,
				Category = new CategoryViewModel()
				{
					Id = post.Category.Id,
					Name = post.Category.Name
				},
				Tags = new List<TagViewModel>()
				{
					new TagViewModel() { Id = 1, Name = "TagName1" },
					new TagViewModel() { Id = 2, Name = "TagName2" },
					new TagViewModel() { Id = 3, Name = "TagName3" }
				},
			};

			var result = Mapper.Map<Post, PostViewModel>(post);

			result.Should().BeEquivalentTo(postViewModelShouldBe, opt => opt.Excluding(pvm => pvm.Tags));
			result.Tags.Should().NotBeNullOrEmpty();
			result.Tags.Should().BeEquivalentTo(postViewModelShouldBe.Tags);
		}
	}
}
