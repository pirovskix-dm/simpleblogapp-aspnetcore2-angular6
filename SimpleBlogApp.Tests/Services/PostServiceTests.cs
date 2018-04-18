using FluentAssertions;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Services;
using SimpleBlogApp.Tests.FakeDependencies;
using SimpleBlogApp.Tests.FakeDependencies.Repositories;
using SimpleBlogApp.ViewModels.QueryViewModels;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SimpleBlogApp.Tests.Services
{
	public class PostServiceTests
	{
		private readonly FakePostRepository fakePostRepository;
		private readonly FakeAutoMapper fakeAutoMapper;
		private readonly PostService postService;
		private readonly ITestOutputHelper output;
		private SavePostViewModel savePost;

		public PostServiceTests(ITestOutputHelper output)
		{
			this.output = output;
			fakePostRepository = new FakePostRepository();
			fakeAutoMapper = new FakeAutoMapper();

			postService = new PostService(fakePostRepository.Object, fakeAutoMapper.Object);

			fakeAutoMapper.Setup();

			savePost = new SavePostViewModel()
			{
				Title = "Save_Title",
				ShortContent = "Save_ShortContent",
				Content = "Save_Content"
			};
		}

		[Fact]
		public async Task GetAllViewModelsAsync_ShouldReturnPostViewModels()
		{
			fakePostRepository.SetupGetTagsAsync();
			var postsFromRep = fakePostRepository.SetupGetAllAsync();

			var result = await postService.GetAllViewModelsAsync();

			result.Should().NotBeNullOrEmpty();
			result.Should().BeAssignableTo<IEnumerable<PostViewModel>>();
			result.Count().Should().Be(postsFromRep.Count());
			result.Select(pvm => pvm.Id).Should().BeEquivalentTo(postsFromRep.Select(p => p.Id));
		}

		[Fact]
		public async Task GetBlogViewModels_ShouldReturnQueryResultWithPostViewModels()
		{
			var queryResult = fakePostRepository.SetupGetQueryResultAsync();
			fakePostRepository.SetupGetTagsAsync(queryResult.Items.Select(p => p.Id));

			var result = await postService.GetBlogViewModels(new PostQueryViewModel());

			result.Should().NotBeNull();
			result.TotalItems.Should().Be(queryResult.TotalItems);
			result.Items.Should().NotBeNullOrEmpty();
			result.Items.Count().Should().Be(queryResult.Items.Count());
			result.Items.Select(pvm => pvm.Id).Should().BeEquivalentTo(queryResult.Items.Select(p => p.Id));
		}

		[Fact]
		public async Task GetAdminViewModels_ShouldReturnQueryResultWithPostViewModels()
		{
			var queryResult = fakePostRepository.SetupGetQueryResultAsync();
			fakePostRepository.SetupGetTagsAsync(queryResult.Items.Select(p => p.Id));

			var result = await postService.GetAdminViewModels(new PostQueryViewModel());

			result.Should().NotBeNull();
			result.TotalItems.Should().Be(queryResult.TotalItems);
			result.Items.Should().NotBeNullOrEmpty();
			result.Items.Count().Should().Be(queryResult.Items.Count());
			result.Items.Select(pvm => pvm.Id).Should().BeEquivalentTo(queryResult.Items.Select(p => p.Id));
		}

		[Fact]
		public async Task GetPostViewModel_PostViewModel()
		{
			var postId = 1;
			fakePostRepository.SetupGetAsync(postId);

			var result = await postService.GetPostViewModel(postId);

			result.Should().NotBeNull();
			result.Id.Should().Be(postId);
		}

		[Fact]
		public async Task UpdatePost_NoPostWithGivenIdExists_ShouldReturnNull()
		{
			fakePostRepository.SetupGetAsync(null);

			var result = await postService.UpdatePost(1, savePost, new[] { new Tag(){} });

			result.Should().BeNull();
		}

		[Fact]
		public async Task UpdatePost_VerifyUpdate()
		{
			int postId = 1;
			fakePostRepository.SetupGetAsync(postId);
			fakePostRepository.SetupUpdate(postId);

			var result = await postService.UpdatePost(postId, savePost, new[] { new Tag() { } });

			fakePostRepository.VerifyUpdate();
		}

		[Fact]
		public async Task UpdatePost_ShouldReturnUpdatedPost()
		{
			int postId = 1;
			fakePostRepository.SetupGetAsync(postId);
			fakePostRepository.SetupUpdate(postId);

			var result = await postService.UpdatePost(postId, savePost, new[] { new Tag() { } });

			result.Should().NotBeNull();
			result.Id.Should().Be(postId);
		}

		[Fact]
		public void AddPost_VerifyAdd()
		{
			fakePostRepository.SetupAdd(1);

			var result = postService.AddPost(savePost, new[] { new Tag() { } });

			fakePostRepository.VerifyAdd();
		}

		[Fact]
		public void AddPost_ShouldReturnAddedPost()
		{
			int postId = 1;
			fakePostRepository.SetupAdd(postId);

			var result = postService.AddPost(savePost, new[] { new Tag() { } });

			result.Should().NotBeNull();
			result.Id.Should().Be(postId);
		}

		[Fact]
		public async Task UpdateOrAddPostIfIdIsNull_ShouldOnlyUpdate()
		{
			int postId = 1;
			fakePostRepository.SetupGetAsync(postId);
			var result = await postService.UpdateOrAddPostIfIdIsNull(postId, savePost, new[] { new Tag() { } });
			fakePostRepository.VerifyUpdateOrAddPostIfIdIsNull(postId);
		}

		[Fact]
		public async Task UpdateOrAddPostIfIdIsNull_ShouldOnlyAdd()
		{
			var result = await postService.UpdateOrAddPostIfIdIsNull(null, savePost, new[] { new Tag() { } });
			fakePostRepository.VerifyUpdateOrAddPostIfIdIsNull(null);
		}

		[Fact]
		public void RemoveVerify()
		{
			int postId = 1;
			postService.Remove(postId);
			fakePostRepository.VerifyRemove(postId);
		}
	}
}
