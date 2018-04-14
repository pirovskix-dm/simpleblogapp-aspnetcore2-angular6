using AutoMapper;
using Moq;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Core.Query;
using SimpleBlogApp.Core.Query.Models;
using SimpleBlogApp.Tests.Extensions;
using SimpleBlogApp.ViewModels.QueryViewModels;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBlogApp.Tests.FakeDependencies
{
	class FakeAutoMapper
	{
		public IMapper Object { get { return mockAutoMapper.Object; } }
		private readonly Mock<IMapper> mockAutoMapper;

		public FakeAutoMapper()
		{
			mockAutoMapper = new Mock<IMapper>();
		}

		public void Setup()
		{
			PostSetup();
			CategorySetup();
			TagSetup();
		}

		private void PostSetup()
		{
			mockAutoMapper.SetupMap((Post p) => new PostViewModel() { Id = p.Id });
			mockAutoMapper.SetupMap((SavePostViewModel sp) => new Post());
			mockAutoMapper.SetupMap((IEnumerable<Post> ps) => ps.Select(p => new PostViewModel() { Id = p.Id }).ToList());
			mockAutoMapper.SetupMap((PostQueryViewModel qvm) => new PostQuery());
			mockAutoMapper.SetupMap((QueryResult<Post> qvm) => new QueryResultViewModel<PostViewModel>() {
				TotalItems = qvm.TotalItems,
				Items = qvm.Items.Select(p => new PostViewModel() { Id = p.Id })
			});
		}

		private void CategorySetup()
		{
			mockAutoMapper.SetupMap((Category c) => new CategoryViewModel() { Id = c.Id });
			mockAutoMapper.SetupMap((IEnumerable<Category> cs) => cs.Select(c => new CategoryViewModel() { Id = c.Id }).ToList());
		}

		private void TagSetup()
		{
			mockAutoMapper.SetupMap((Tag c) => new TagViewModel() { Id = c.Id });
		}
	}
}
