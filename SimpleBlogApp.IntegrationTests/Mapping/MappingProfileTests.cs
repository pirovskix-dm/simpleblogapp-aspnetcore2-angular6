using AutoMapper;
using FluentAssertions;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Mapping;
using SimpleBlogApp.ViewModels.SaveViewModels;
using System;
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
			var savePost = new SavePostViewModel()
			{
				Title = "Test title"
			};

			var result = Mapper.Map<SavePostViewModel, Post>(savePost);

			result.Should().NotBeNull();
			result.Title.Should().Be(savePost.Title);
		}
	}
}
