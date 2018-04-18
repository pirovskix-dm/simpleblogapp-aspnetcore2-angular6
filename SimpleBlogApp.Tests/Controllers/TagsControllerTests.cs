using FluentAssertions;
using SimpleBlogApp.Controllers;
using SimpleBlogApp.Tests.FakeDependencies.Services;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleBlogApp.Tests.Controllers
{
	public class TagsControllerTests
	{
		private readonly FakeTagService fakeTagService;
		private readonly TagsController validController;

		public TagsControllerTests()
		{
			fakeTagService = new FakeTagService();
			validController = new TagsController(fakeTagService.Object);
		}

		[Fact]
		public async Task GetCategories_ValidRequest_ListOfCategoryViewModels()
		{
			var searchName = "name";
			int numOfRecords = 2;
			var tagsFromServ = fakeTagService.SetupFindFirsTagsLike(searchName, numOfRecords);

			var result = await validController.GetTags(searchName, numOfRecords);

			result.Should().NotBeNullOrEmpty();
			result.Should().BeAssignableTo<IEnumerable<TagViewModel>>();
			result.Count().Should().Be(tagsFromServ.Count());
			result.Select(tvm => tvm.Id).Should().BeEquivalentTo(tagsFromServ.Select(t => t.Id));
		}
	}
}
