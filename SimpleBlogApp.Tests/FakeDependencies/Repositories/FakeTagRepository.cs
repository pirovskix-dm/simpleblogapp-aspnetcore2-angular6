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
	class FakeTagRepository
	{
		public ITagRepository Object { get { return mockTagRepository.Object; } }
		private readonly Mock<ITagRepository> mockTagRepository;

		public FakeTagRepository()
		{
			mockTagRepository = new Mock<ITagRepository>();
		}

		public IEnumerable<Tag> SetupFindByNamesAsync(IEnumerable<string> names)
		{
			var tags = names.Select(name => new Tag() { Name = name });
			mockTagRepository
				.Setup(r => r.FindByNamesAsync(names, It.IsAny<Expression<Func<Tag, Tag>>>()))
				.ReturnsAsync(tags);
			return tags;
		}

		public IEnumerable<TagViewModel> SetupFindByNameAsync(int records)
		{
			var tags = CreateTagViewModels(records);
			mockTagRepository
				.Setup(r => r.FindByNameAsync(It.IsAny<string>(), records, It.IsAny<Expression<Func<Tag, TagViewModel>>>()))
				.ReturnsAsync(tags);
			return tags;
		}

		public void VerifyAddRange(bool should)
		{
			mockTagRepository.Verify(r => r.AddRange(It.IsAny<IEnumerable<Tag>>()), should ? Times.Once() : Times.Never());
		}

		public IEnumerable<Tag> CreateTags()
		{
			return Enumerable.Range(1, 10).Select(x => new Tag() { Id = x });
		}

		public IEnumerable<TagViewModel> CreateTagViewModels(int numOfRecords)
		{
			return Enumerable.Range(1, numOfRecords).Select(x => new TagViewModel() { Id = x });
		}
	}
}
