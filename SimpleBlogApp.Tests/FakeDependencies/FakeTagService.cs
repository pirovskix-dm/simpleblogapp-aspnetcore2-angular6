using Moq;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Services.Interfaces;
using System.Collections.Generic;

namespace SimpleBlogApp.Tests.FakeDependencies
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
	}
}
