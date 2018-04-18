using Moq;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Services.Interfaces;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBlogApp.Tests.FakeDependencies.Services
{
	class FakeCategoryService
	{
		private readonly Mock<ICategoryService> mockCategoryService;
		public ICategoryService Object { get { return mockCategoryService.Object; } }

		public FakeCategoryService()
		{
			mockCategoryService = new Mock<ICategoryService>();
		}

		public IEnumerable<CategoryViewModel> SetupGetAllViewModelsAsync()
		{
			var categories = CreateCategories();

			mockCategoryService
				.Setup(s => s.GetAllViewModelsAsync())
				.ReturnsAsync(categories);

			return categories;
		}

		public Category SetupAddIfNotExists(int createdId, bool exists = false)
		{
			var category = exists ? null : new Category() { Id = createdId };

			mockCategoryService
				.Setup(s => s.AddIfNotExists(It.IsAny<SaveCategoryViewModel>()))
				.ReturnsAsync(category);

			return category;
		}

		public void VerifyAddIfNotExists()
		{
			mockCategoryService.Verify(s => s.AddIfNotExists(It.IsAny<SaveCategoryViewModel>()));
		}

		public void VerifyRemove(int categoryId)
		{
			mockCategoryService.Verify(s => s.Remove(It.Is<int>(id => id == categoryId)), Times.Once());
		}

		private IEnumerable<CategoryViewModel> CreateCategories()
		{
			return Enumerable.Range(1, 10).Select(x => new CategoryViewModel() { Id = x });
		}
	}
}
