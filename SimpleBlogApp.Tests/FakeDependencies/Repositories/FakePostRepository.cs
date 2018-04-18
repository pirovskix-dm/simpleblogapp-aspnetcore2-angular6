using Moq;
using SimpleBlogApp.Core.Interfaces;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Core.Query;
using SimpleBlogApp.Core.Query.Models;
using SimpleBlogApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace SimpleBlogApp.Tests.FakeDependencies.Repositories
{
	using IdObjTagVM = Expression<Func<IdObject<Tag>, IdObject<TagViewModel>>>;
	using PostExp = Expression<Func<Post, Post>>;
	using PostVMExp = Expression<Func<Post, PostViewModel>>;

	class FakePostRepository
	{
		public IPostRepository Object { get { return mockPostRepository.Object; } }
		private readonly Mock<IPostRepository> mockPostRepository;

		private Expression<Action<IPostRepository>> expAdd;
		private Expression<Action<IPostRepository>> expUpdate;
		private Expression<Action<IPostRepository>> badUpdate;

		public FakePostRepository()
		{
			mockPostRepository = new Mock<IPostRepository>();
		}

		public IEnumerable<PostViewModel> SetupGetAllAsync()
		{
			var posts = CreatePostViewModels();
			mockPostRepository
				.Setup(r => r.GetAllAsync(It.IsAny<PostVMExp>()))
				.ReturnsAsync(posts);
			return posts;
		}

		public IEnumerable<IdObject<TagViewModel>> SetupGetTagsAsync()
		{
			var tags = CreateIdObjectTag(new[] { 1, 2, 3, 4, 5 });
			mockPostRepository
				.Setup(r => r.GetTagsAsync(It.IsAny<IdObjTagVM>()))
				.ReturnsAsync(tags);
			return tags;
		}

		public IEnumerable<IdObject<TagViewModel>> SetupGetTagsAsync(int id)
		{
			var tags = CreateIdObjectTag(new[] { id });
			mockPostRepository
				.Setup(r => r.GetTagsAsync(id, It.IsAny<IdObjTagVM>()))
				.ReturnsAsync(tags);
			return tags;
		}

		public IEnumerable<IdObject<TagViewModel>> SetupGetTagsAsync(IEnumerable<int> idList)
		{
			var tags = CreateIdObjectTag(idList);
			mockPostRepository
				.Setup(r => r.GetTagsAsync(idList, It.IsAny<IdObjTagVM>()))
				.ReturnsAsync(tags);
			return tags;
		}

		public QueryResult<PostViewModel> SetupGetQueryResultAsync()
		{
			var queryResult = new QueryResult<PostViewModel>()
			{
				TotalItems = 20,
				Items = CreatePostViewModels()
			};
			mockPostRepository
				.Setup(r => r.GetQueryResultAsync(It.IsAny<PostQuery>(), It.IsAny<PostVMExp>()))
				.ReturnsAsync(queryResult);
			return queryResult;
		}

		public void SetupGetAsync(int? postId)
		{
			PostViewModel postVM = null;
			Post post = null;
			if (postId.HasValue)
			{
				postVM = new PostViewModel() { Id = postId.Value };
				post = new Post() { Id = postId.Value };
				mockPostRepository.Setup(r => r.GetAsync(postId.Value, It.IsAny<PostVMExp>())).ReturnsAsync(postVM);
				mockPostRepository.Setup(r => r.GetAsync(postId.Value, It.IsAny<PostExp>())).ReturnsAsync(post);
			}
			else
			{
				mockPostRepository.Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<PostVMExp>())).ReturnsAsync(postVM);
				mockPostRepository.Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<PostExp>())).ReturnsAsync(post);
			}
		}

		public void SetupAdd(int createdId)
		{
			expAdd = (r => r.Add(It.IsAny<Post>()));
			mockPostRepository
				.Setup(expAdd)
				.Callback((Post p) => { p.Id = createdId; });
		}

		public void VerifyAdd()
		{
			mockPostRepository.Verify(expAdd, Times.Once());
		}

		public void SetupUpdate(int postId)
		{
			expUpdate = (r => r.Update(It.Is<Post>(p => p.Id == postId)));
			badUpdate = (r => r.Update(It.Is<Post>(p => p.Id != postId)));
			mockPostRepository.Setup(expUpdate);
		}

		public void VerifyUpdate()
		{
			mockPostRepository.Verify(expUpdate, Times.Once());
			mockPostRepository.Verify(badUpdate, Times.Never());
		}

		public void VerifyUpdateOrAddPostIfIdIsNull(int? id)
		{
			if (id.HasValue)
			{
				mockPostRepository.Verify(r => r.Update(It.IsAny<Post>()), Times.Once());
				mockPostRepository.Verify(r => r.Add(It.IsAny<Post>()), Times.Never());
			}
			else
			{
				mockPostRepository.Verify(r => r.Update(It.IsAny<Post>()), Times.Never());
				mockPostRepository.Verify(r => r.Add(It.IsAny<Post>()), Times.Once());
			}
		}

		public void VerifyRemove(int postId)
		{
			mockPostRepository.Verify(r => r.Remove(It.Is<Post>(p => p.Id == postId)), Times.Once());
			mockPostRepository.Verify(r => r.Remove(It.Is<Post>(p => p.Id != postId)), Times.Never());
		}

		private IEnumerable<Post> CreatePosts()
		{
			return Enumerable.Range(1, 10).Select(x => new Post() { Id = x });
		}

		private IEnumerable<PostViewModel> CreatePostViewModels()
		{
			return Enumerable.Range(1, 10).Select(x => new PostViewModel() { Id = x });
		}

		private IEnumerable<IdObject<TagViewModel>> CreateIdObjectTag(IEnumerable<int> idList)
		{
			return idList.Select(id => new IdObject<TagViewModel>() {
				Id = id,
				Object = new TagViewModel() { Id = id }
			});
		}
	}
}
