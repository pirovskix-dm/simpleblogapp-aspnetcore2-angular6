using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleBlogApp.Controllers;
using SimpleBlogApp.Tests.FakeDependencies.Services;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SimpleBlogApp.Tests.Controllers
{
	public class CategoriesControllerTests
	{
		private const string modelStateErrorKey = "ErrorKey";
		private const string modelStateErrorMessage = "Error Message";

		private readonly ITestOutputHelper output;
		private readonly FakeUnitOfWorkService fakeUnitOfWorkService;
		private readonly FakeCategoryService fakeCategoryService;
		private readonly CategoriesController validController;
		private readonly CategoriesController errorController;

		private SaveCategoryViewModel saveCategory;

		public CategoriesControllerTests(ITestOutputHelper output)
		{
			this.output = output;
			fakeUnitOfWorkService = new FakeUnitOfWorkService();
			fakeCategoryService = new FakeCategoryService();

			validController = new CategoriesController(fakeUnitOfWorkService.Object, fakeCategoryService.Object);
			validController.ModelState.Clear();

			errorController = new CategoriesController(fakeUnitOfWorkService.Object, fakeCategoryService.Object);
			errorController.ModelState.AddModelError(modelStateErrorKey, modelStateErrorMessage);

			saveCategory = new SaveCategoryViewModel()
			{
				Name = "Save_Category"
			};
		}

		[Fact]
		public async Task GetCategories_ValidRequest_ListOfCategoryViewModels()
		{
			var categoriesFromServ = fakeCategoryService.SetupGetAllViewModelsAsync();

			var result = await validController.GetCategories();

			result.Should().NotBeNullOrEmpty();
			result.Should().BeAssignableTo<IEnumerable<CategoryViewModel>>();
			result.Count().Should().Be(categoriesFromServ.Count());
			result.Select(cvm => cvm.Id).Should().BeEquivalentTo(categoriesFromServ.Select(c => c.Id));
		}

		[Fact]
		public async Task CreateCategory_ModelStateIsNotValid_BadRequestWithModelState()
		{
			var result = await errorController.CreateCategory(saveCategory);

			var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
			var modelError = badRequestResult.Value.Should().BeAssignableTo<SerializableError>().Subject;
			modelError.Should().ContainKey(modelStateErrorKey);
		}

		[Fact]
		public async Task CreateCategory_CategoryAlreadyExists_BadRequestWithModelState()
		{
			fakeUnitOfWorkService.SetupTrySaveChangesAsync(true);
			fakeCategoryService.SetupAddIfNotExists(1, true);

			var result = await errorController.CreateCategory(saveCategory);

			var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
			badRequestResult.Value.Should().BeAssignableTo<SerializableError>();
		}

		[Fact]
		public async Task CreateCategory_VerifyAddIfNotExists()
		{
			fakeCategoryService.SetupAddIfNotExists(1);
			await validController.CreateCategory(saveCategory);
			fakeCategoryService.VerifyAddIfNotExists();
		}

		[Fact]
		public async Task CreateCategory_VerifySaveAsync()
		{
			fakeCategoryService.SetupAddIfNotExists(1);
			await validController.CreateCategory(saveCategory);
			fakeUnitOfWorkService.VerifySave();
		}

		[Fact]
		public async Task CreateCategory_FaildToSave_ObjectResultWithStatus500()
		{
			fakeCategoryService.SetupAddIfNotExists(1);
			fakeUnitOfWorkService.SetupTrySaveChangesAsync(false);

			var result = await validController.CreateCategory(saveCategory);

			var objectResult = result.Should().BeOfType<StatusCodeResult>().Subject;
			objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
		}

		[Fact]
		public async Task CreatePost_ValidRequest_OkResultWithTheIdOfTheCreatedCategory()
		{
			var createdId = 2;
			fakeCategoryService.SetupAddIfNotExists(createdId);
			fakeUnitOfWorkService.SetupTrySaveChangesAsync(true);

			var result = await validController.CreateCategory(saveCategory);

			var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
			var resultId = okObjectResult.Value.Should().BeOfType<int>().Subject;
			resultId.Should().Be(createdId);
		}

		[Fact]
		public async Task DeleteCategory_VerifyRemove()
		{
			var categoryId = 1;
			await validController.DeleteCategory(categoryId);
			fakeCategoryService.VerifyRemove(categoryId);
		}

		[Fact]
		public async Task DeleteCategory_VerifySaveAsync()
		{
			await validController.DeleteCategory(1);
			fakeUnitOfWorkService.VerifySave();
		}

		[Fact]
		public async Task DeleteCategory_ValidRequest_OkObjectResultWithIdOfTheDeletedPost()
		{
			var categoryId = 2;
			fakeUnitOfWorkService.SetupTrySaveChangesAsync(true);

			var result = await validController.DeleteCategory(categoryId);

			var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
			var resultId = okObjectResult.Value.Should().BeOfType<int>().Subject;
			resultId.Should().Be(categoryId);
		}
	}
}
