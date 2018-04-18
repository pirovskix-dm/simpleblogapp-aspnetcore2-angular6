using FluentAssertions;
using SimpleBlogApp.Services;
using SimpleBlogApp.Tests.FakeDependencies.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SimpleBlogApp.Tests.Services
{
	public class TagServiceTests
	{
		private readonly FakeTagRepository fakeTagRepository;
		private readonly TagService tagService;
		private readonly ITestOutputHelper output;

		public TagServiceTests(ITestOutputHelper output)
		{
			this.output = output;
			fakeTagRepository = new FakeTagRepository();
			tagService = new TagService(fakeTagRepository.Object);
		}

		[Fact]
		public async Task FindByNamesAndAddIfNotExists_VerifyAddRange_ShouldBeCalled()
		{
			fakeTagRepository.SetupFindByNamesAsync(new[] { "Tag1" });
			await tagService.FindByNamesAndAddIfNotExists(new[] { "Tag1", "Tag2", "Tag3" });
			fakeTagRepository.VerifyAddRange(true);
		}

		[Fact]
		public async Task FindByNamesAndAddIfNotExists_VerifyAddRange_ShouldNotBeCalled()
		{
			fakeTagRepository.SetupFindByNamesAsync(new[] { "Tag1" });
			await tagService.FindByNamesAndAddIfNotExists(new[] { "Tag1" });
			fakeTagRepository.VerifyAddRange(false);
		}

		[Fact]
		public async Task FindByNamesAndAddIfNotExists_ShoudlReturnTags()
		{
			var saveTags = new[] { "Tag1", "Tag2", "Tag3" };
			fakeTagRepository.SetupFindByNamesAsync(new[] { "Tag1" });

			var result = await tagService.FindByNamesAndAddIfNotExists(saveTags);

			result.Should().NotBeNullOrEmpty();
			result.Count().Should().Be(saveTags.Length);
		}

		[Fact]
		public async Task FindFirsTagsLike_ShoudlReturnTagViewModels()
		{
			int records = 5;
			var tagsFromRep = fakeTagRepository.SetupFindByNameAsync(records);

			var result = await tagService.FindFirsTagsLike("name", records);

			result.Should().NotBeNullOrEmpty();
			result.Count().Should().Be(records);
			result.Select(t => t.Id).Should().BeEquivalentTo(tagsFromRep.Select(t => t.Id));
		}
	}
}
