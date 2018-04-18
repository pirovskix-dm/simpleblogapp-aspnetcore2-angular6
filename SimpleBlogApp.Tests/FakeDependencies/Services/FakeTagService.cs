using Moq;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Services.Interfaces;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBlogApp.Tests.FakeDependencies.Services
{
	class FakeTagService
	{
		private readonly Mock<ITagService> mockTagService;
		public ITagService Object { get { return mockTagService.Object; } }

		public FakeTagService()
		{
			mockTagService = new Mock<ITagService>();
		}

		public IEnumerable<Tag> SetupFindByNamesAndAddIfNotExists()
		{
			var tags = new List<Tag>();

			mockTagService
				.Setup(s => s.FindByNamesAndAddIfNotExists(It.IsAny<ICollection<string>>()))
				.ReturnsAsync(tags);

			return tags;
		}

		public IEnumerable<TagViewModel> SetupFindFirsTagsLike(string name, int numOfRecords)
		{
			var tags = CreateCategories(numOfRecords);

			mockTagService
				.Setup(s => s.FindFirsTagsLike(name, numOfRecords))
				.ReturnsAsync(tags);

			return tags;
		}

		private IEnumerable<TagViewModel> CreateCategories(int numOfRecords)
		{
			return Enumerable.Range(1, numOfRecords).Select(x => new TagViewModel() { Id = x });
		}
	}
}
