using FluentAssertions;
using SimpleBlogApp.Services;
using SimpleBlogApp.Tests.FakeDependencies;
using SimpleBlogApp.Tests.FakeDependencies.Repositories;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SimpleBlogApp.Tests.Services
{
	public class CategoryServiceTests
	{
		private readonly FakeCategoryRepository fakeCategoryRepository;
		private readonly FakeAutoMapper fakeAutoMapper;
		private readonly CategoryService categoryService;
		private readonly ITestOutputHelper output;
		private SaveCategoryViewModel saveCategory;

		public CategoryServiceTests(ITestOutputHelper output)
		{
			this.output = output;
			fakeCategoryRepository = new FakeCategoryRepository();
			fakeAutoMapper = new FakeAutoMapper();

			categoryService = new CategoryService(fakeCategoryRepository.Object, fakeAutoMapper.Object);

			fakeAutoMapper.Setup();

			saveCategory = new SaveCategoryViewModel()
			{
				Name = "Save_Name"
			};
		}

		[Fact]
		public async Task GetAllViewModelsAsync_ShouldReturnCategoryViewModel()
		{
			var categoriesFromRep = fakeCategoryRepository.SetupGetAllAsync();

			var result = await categoryService.GetAllViewModelsAsync();

			result.Should().NotBeNullOrEmpty();
			result.Should().BeAssignableTo<IEnumerable<CategoryViewModel>>();
			result.Count().Should().Be(categoriesFromRep.Count());
			result.Select(pvm => pvm.Id).Should().BeEquivalentTo(categoriesFromRep.Select(c => c.Id));
		}

		[Fact]
		public async Task AddIfNotExists_CategoryWithGivenIdExists_ShouldReturnNull()
		{
			fakeCategoryRepository.SetupIsExistAsync(true);
			var result = await categoryService.AddIfNotExists(saveCategory);
			result.Should().BeNull();
		}

		[Fact]
		public async Task AddIfNotExists_VerifyAdd()
		{
			fakeCategoryRepository.SetupIsExistAsync(false);
			fakeCategoryRepository.SetupAdd(1);
			var result = await categoryService.AddIfNotExists(saveCategory);
			fakeCategoryRepository.VerifyAdd();
		}

		[Fact]
		public async Task AddIfNotExists_ShouldReturnAddedCategory()
		{
			int categoryId = 1;
			fakeCategoryRepository.SetupIsExistAsync(false);
			fakeCategoryRepository.SetupAdd(categoryId);

			var result = await categoryService.AddIfNotExists(saveCategory);

			result.Should().NotBeNull();
			result.Id.Should().Be(categoryId);
		}

		[Fact]
		public void RemoveVerify()
		{
			int categoryId = 1;
			categoryService.Remove(categoryId);
			fakeCategoryRepository.VerifyRemove(categoryId);
		}
	}
}
