using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleBlogApp.Controllers;
using SimpleBlogApp.Tests.FakeDependencies.Services;
using SimpleBlogApp.ViewModels.QueryViewModels;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SimpleBlogApp.Tests.Controllers
{
	public class PostsControllerTests
	{
		
		private const string modelStateErrorKey = "ErrorKey";
		private const string modelStateErrorMessage = "Error Message";

		private readonly ITestOutputHelper output;
		private readonly FakeUnitOfWorkService fakeUnitOfWorkService;
		private readonly FakePostService fakePostService;
		private readonly FakeTagService fakeTagService;
		private readonly PostsController validController;
		private readonly PostsController errorController;

		private SavePostViewModel savePost;

		public PostsControllerTests(ITestOutputHelper output)
		{
			this.output = output;
			fakeUnitOfWorkService = new FakeUnitOfWorkService();
			fakePostService = new FakePostService();
			fakeTagService = new FakeTagService();

			validController = new PostsController(fakeUnitOfWorkService.Object, fakePostService.Object, fakeTagService.Object);
			validController.ModelState.Clear();

			errorController = new PostsController(fakeUnitOfWorkService.Object, fakePostService.Object, fakeTagService.Object);
			errorController.ModelState.AddModelError(modelStateErrorKey, modelStateErrorMessage);

			savePost = new SavePostViewModel()
			{
				Title = "Save_Title",
				ShortContent = "Save_ShortContent",
				Content = "Save_Content"
			};
		}

		[Fact]
		public async Task GetPosts_ValidRequest_ListOfPostViewModels()
		{
			var postsFromSer = fakePostService.SetupGetAllViewModelsAsync();

			var result = await validController.GetPosts();

			result.Should().NotBeNullOrEmpty();
			result.Should().BeAssignableTo<IEnumerable<PostViewModel>>();
			result.Count().Should().Be(postsFromSer.Count());
			result.Select(pvm => pvm.Id).Should().BeEquivalentTo(postsFromSer.Select(p => p.Id));
		}

		[Fact]
		public async Task GetPostsQuery_ValidRequest_QueryResultViewModel()
		{
			var queryResultFromSer = fakePostService.SetupGetBlogViewModels();

			var result = await validController.GetPostsQuery(new PostQueryViewModel());

			result.Should().NotBeNull();
			result.TotalItems.Should().Be(queryResultFromSer.TotalItems);
			result.Items.Should().NotBeNullOrEmpty();
			result.Items.Count().Should().Be(queryResultFromSer.Items.Count());
			result.Items.Select(pvm => pvm.Id).Should().BeEquivalentTo(queryResultFromSer.Items.Select(p => p.Id));
		}

		[Fact]
		public async Task GetAdminQuery_ValidRequest_QueryResultViewModel()
		{
			var queryResultFromSer = fakePostService.SetupGetAdminViewModels();

			var result = await validController.GetAdminQuery(new PostQueryViewModel());

			result.Should().NotBeNull();
			result.TotalItems.Should().Be(queryResultFromSer.TotalItems);
			result.Items.Should().NotBeNullOrEmpty();
			result.Items.Count().Should().Be(queryResultFromSer.Items.Count());
			result.Items.Select(pvm => pvm.Id).Should().BeEquivalentTo(queryResultFromSer.Items.Select(p => p.Id));
		}

		[Fact]
		public async Task GetPost_ValidRequest_OkObjectResultWithPostViewModel()
		{
			int postId = 1;
			var postFromRep = fakePostService.SetupGetPostViewModel(postId);

			var result = await validController.GetPost(postId);

			var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
			var postViewModel = okObjectResult.Value.Should().BeOfType<PostViewModel>().Subject;
			postViewModel.Id.Should().Be(postId);
		}

		[Fact]
		public async Task GetPost_NoPostWithGivenIdExists_NotFoundResult()
		{
			fakePostService.SetupGetPostViewModel(null);

			var result = await validController.GetPost(2);

			result.Should().BeOfType<NotFoundResult>();
		}

		[Fact]
		public async Task CreatePost_ModelStateIsNotValid_BadRequestObjectResultWithModelState()
		{
			fakeUnitOfWorkService.SetupTrySaveChangesAsync(true);

			var result = await errorController.CreatePost(savePost);

			var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
			var modelError = badRequestResult.Value.Should().BeAssignableTo<SerializableError>().Subject;
			modelError.Should().ContainKey(modelStateErrorKey);
		}

		[Fact]
		public async Task CreatePost_VerifyAdd()
		{
			fakePostService.SetupUpdateOrAddPostIfIdIsNull(null, 1);

			await validController.CreatePost(savePost);
			
			fakePostService.VerifyUpdateOrAddPostIfIdIsNull(null);
		}

		[Fact]
		public async Task CreatePost_VerifySaveAsync()
		{
			fakePostService.SetupUpdateOrAddPostIfIdIsNull(null, 1);
			await validController.CreatePost(savePost);
			fakeUnitOfWorkService.VerifySave();
		}

		[Fact]
		public async Task CreatePost_ValidRequest_OkObjectResultWithTheIdOfTheCreatedPost()
		{
			var createdId = 1;
			fakePostService.SetupUpdateOrAddPostIfIdIsNull(null, createdId);
			fakeUnitOfWorkService.SetupTrySaveChangesAsync(true);

			var result = await validController.CreatePost(savePost);

			var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
			var resultId = okObjectResult.Value.Should().BeOfType<int>().Subject;
			resultId.Should().Be(createdId);
		}

		[Fact]
		public async Task CreatePost_FaildToSave_ObjectResultWithStatus500()
		{
			var createdId = 1;
			fakePostService.SetupUpdateOrAddPostIfIdIsNull(null, createdId);
			fakeUnitOfWorkService.SetupTrySaveChangesAsync(false);

			var result = await validController.CreatePost(savePost);

			var objectResult = result.Should().BeOfType<StatusCodeResult>().Subject;
			objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
		}

		[Fact]
		public async Task UpdatePost_ModelStateIsNotValid_BadRequestObjectResultWithModelState()
		{
			var result = await errorController.UpdatePost(1, savePost);

			var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
			var modelError = badRequestResult.Value.Should().BeAssignableTo<SerializableError>().Subject;
			modelError.Should().ContainKey(modelStateErrorKey);
		}

		[Fact]
		public async Task UpdatePost_NoPostWithGivenIdExists_NotFoundResult()
		{
			var postId = 1;
			fakePostService.SetupUpdateOrAddPostIfIdIsNull(postId, 0, true);

			var result = await validController.UpdatePost(postId, savePost);

			result.Should().BeOfType<NotFoundResult>();
		}

		[Fact]
		public async Task UpdatePost_VerifyUpdate()
		{
			var postId = 1;
			fakePostService.SetupUpdateOrAddPostIfIdIsNull(postId, 0);

			await validController.UpdatePost(postId, savePost);

			fakePostService.VerifyUpdateOrAddPostIfIdIsNull(postId);
		}

		[Fact]
		public async Task UpdatePost_VerifySaveAsync()
		{
			int postId = 1;
			fakePostService.SetupUpdateOrAddPostIfIdIsNull(postId, 0);

			await validController.UpdatePost(postId, savePost);

			fakeUnitOfWorkService.VerifySave();
		}

		[Fact]
		public async Task UpdatePost_FaildToSave_ObjectResultWithStatus500()
		{
			var postId = 1;
			fakePostService.SetupUpdateOrAddPostIfIdIsNull(postId, 0);
			fakeUnitOfWorkService.SetupTrySaveChangesAsync(false);

			var result = await validController.UpdatePost(postId, savePost);

			var objectResult = result.Should().BeOfType<StatusCodeResult>().Subject;
			objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
		}

		[Fact]
		public async Task UpdatePost_ValidRequest_OkObjectResultWithTheIdOfTheUpdatedPost()
		{
			int postId = 2;
			fakePostService.SetupUpdateOrAddPostIfIdIsNull(postId, 0);
			fakeUnitOfWorkService.SetupTrySaveChangesAsync(true);

			var result = await validController.UpdatePost(postId, savePost);

			var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
			var resultId = okObjectResult.Value.Should().BeOfType<int>().Subject;
			resultId.Should().Be(postId);
		}

		[Fact]
		public async Task DeletePost_VerifyRemove()
		{
			var postId = 1;

			await validController.DeletePost(postId);

			fakePostService.VerifyRemove(postId);
		}

		[Fact]
		public async Task DeletePost_VerifySaveAsync()
		{
			await validController.DeletePost(1);
			fakeUnitOfWorkService.VerifySave();
		}

		[Fact]
		public async Task DeletePost_ValidRequest_OkObjectResultWithIdOfTheDeletedPost()
		{
			var postId = 2;
			fakeUnitOfWorkService.SetupTrySaveChangesAsync(true);

			var result = await validController.DeletePost(postId);

			var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
			var resultId = okObjectResult.Value.Should().BeOfType<int>().Subject;
			resultId.Should().Be(postId);
		}
		
	}
}