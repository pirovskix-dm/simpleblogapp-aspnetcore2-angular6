using Moq;
using SimpleBlogApp.Services.Interfaces;

namespace SimpleBlogApp.Tests.FakeDependencies
{
	class FakeUnitOfWorkService
	{
		public IUnitOfWorkService Object { get { return mockUnitOfWork.Object; } }
		private readonly Mock<IUnitOfWorkService> mockUnitOfWork;

		public FakeUnitOfWorkService()
		{
			mockUnitOfWork = new Mock<IUnitOfWorkService>();
		}

		public void SetupTrySaveChangesAsync(bool isSuccess)
		{
			mockUnitOfWork.Setup(u => u.TrySaveChangesAsync()).ReturnsAsync(isSuccess);
		}

		public void VerifySave()
		{
			mockUnitOfWork.Verify(u => u.TrySaveChangesAsync(), Times.AtLeastOnce());
		}
	}
}