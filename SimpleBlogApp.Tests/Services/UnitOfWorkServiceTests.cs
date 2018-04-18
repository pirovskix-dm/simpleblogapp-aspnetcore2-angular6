using FluentAssertions;
using SimpleBlogApp.Services;
using SimpleBlogApp.Tests.FakeDependencies.Repositories;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SimpleBlogApp.Tests.Services
{
	public class UnitOfWorkServiceTests
	{
		private readonly FakeUnitOfWork fakeUnitOfWork;
		private readonly UnitOfWorkService uowService;
		private readonly ITestOutputHelper output;

		public UnitOfWorkServiceTests(ITestOutputHelper output)
		{
			this.output = output;
			fakeUnitOfWork = new FakeUnitOfWork();
			uowService = new UnitOfWorkService(fakeUnitOfWork.Object);
		}

		[Fact]
		public async Task TrySaveChangesAsync_SaveFail_False()
		{
			fakeUnitOfWork.SetupSaveAsyncFail();
			var result = await uowService.TrySaveChangesAsync();
			result.Should().BeFalse();
		}

		[Fact]
		public async Task TrySaveChangesAsync_ShouldReturnTrue()
		{
			var result = await uowService.TrySaveChangesAsync();
			result.Should().BeTrue();
		}
	}
}
