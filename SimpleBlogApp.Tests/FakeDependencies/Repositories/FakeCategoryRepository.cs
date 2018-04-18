using Moq;
using SimpleBlogApp.Core.Interfaces;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SimpleBlogApp.Tests.FakeDependencies.Repositories
{
	class FakeCategoryRepository
	{
		public ICategoryRepository Object { get { return mockCategoryRepository.Object; } }
		private readonly Mock<ICategoryRepository> mockCategoryRepository;

		private Expression<Action<ICategoryRepository>> expAdd;

		public FakeCategoryRepository()
		{
			mockCategoryRepository = new Mock<ICategoryRepository>();
		}

		public IEnumerable<CategoryViewModel> SetupGetAllAsync()
		{
			var category = CreatePostViewModels();
			mockCategoryRepository
				.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Category, CategoryViewModel>>>()))
				.ReturnsAsync(category);
			return category;
		}

		public void SetupIsExistAsync(bool exists)
		{
			mockCategoryRepository.Setup(r => r.IsExistAsync(It.IsAny<string>())).ReturnsAsync(exists);
		}

		public void SetupAdd(int createdId)
		{
			expAdd = (r => r.Add(It.IsAny<Category>()));
			mockCategoryRepository
				.Setup(expAdd)
				.Callback((Category c) => { c.Id = createdId; });
		}

		public void VerifyAdd()
		{
			mockCategoryRepository.Verify(expAdd, Times.Once());
		}

		public void VerifyRemove(int categoryId)
		{
			mockCategoryRepository.Verify(r => r.Remove(It.Is<Category>(c => c.Id == categoryId)), Times.Once());
			mockCategoryRepository.Verify(r => r.Remove(It.Is<Category>(c => c.Id != categoryId)), Times.Never());
		}

		private IEnumerable<CategoryViewModel> CreatePostViewModels()
		{
			return Enumerable.Range(1, 10).Select(x => new CategoryViewModel() { Id = x });
		}
	}
}
