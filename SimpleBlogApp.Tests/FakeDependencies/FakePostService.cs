using Moq;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Core.Query;
using SimpleBlogApp.Services.Interfaces;
using SimpleBlogApp.ViewModels.QueryViewModels;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBlogApp.Tests.FakeDependencies
{
	class FakePostService
	{
		private readonly Mock<IPostService> mockPostService;
		public IPostService Object { get { return mockPostService.Object; } }

		public FakePostService()
		{
			mockPostService = new Mock<IPostService>();
		}

		public IEnumerable<PostViewModel> SetupGetAllViewModelsAsync()
		{
			var posts = CreatePostViewModels();

			mockPostService
				.Setup(s => s.GetAllViewModelsAsync())
				.ReturnsAsync(posts);

			return posts;
		}

		public QueryResult<PostViewModel> SetupGetBlogViewModels()
		{
			var queryResul = new QueryResult<PostViewModel>()
			{
				TotalItems = 20,
				Items = CreatePostViewModels()
			};

			mockPostService
				.Setup(s => s.GetBlogViewModels(It.IsAny<PostQueryViewModel>()))
				.ReturnsAsync(queryResul);

			return queryResul;
		}

		public QueryResult<PostViewModel> SetupGetAdminViewModels()
		{
			var queryResul = new QueryResult<PostViewModel>()
			{
				TotalItems = 20,
				Items = CreatePostViewModels()
			};

			mockPostService
				.Setup(s => s.GetAdminViewModels(It.IsAny<PostQueryViewModel>()))
				.ReturnsAsync(queryResul);

			return queryResul;
		}

		public PostViewModel SetupGetPostViewModel(int? postId)
		{
			PostViewModel post = null;
			if (postId.HasValue)
			{
				post = new PostViewModel() { Id = postId.Value };
				mockPostService.Setup(s => s.GetPostViewModel(postId.Value)).ReturnsAsync(post);
			}
			else
			{
				mockPostService.Setup(s => s.GetPostViewModel(It.IsAny<int>())).ReturnsAsync(post);
			}
			return post;
		}

		public Post SetupUpdateOrAddPostIfIdIsNull(int? postId, int createdId, bool isNotFound = false)
		{
			if (isNotFound)
				return null;

			var post = new Post() { Id = postId.HasValue ? postId.Value : createdId };

			mockPostService
				.Setup(s => s.UpdateOrAddPostIfIdIsNull(postId, It.IsAny<SavePostViewModel>(), It.IsAny<IEnumerable<Tag>>()))
				.ReturnsAsync(post);

			return post;
		}

		public void VerifyUpdateOrAddPostIfIdIsNull(int? postId)
		{
			mockPostService.Verify(s => s.UpdateOrAddPostIfIdIsNull(It.Is<int?>(id => id == postId), It.IsAny<SavePostViewModel>(), It.IsAny<IEnumerable<Tag>>()), Times.Once());
		}

		public void VerifyRemove(int postId)
		{
			mockPostService.Verify(s => s.Remove(It.Is<int>(id => id == postId)), Times.Once());
		}

		private IEnumerable<PostViewModel> CreatePostViewModels()
		{
			return Enumerable.Range(1, 10).Select(x => new PostViewModel() { Id = x });
		}
	}
}
