using Microsoft.EntityFrameworkCore;
using Moq;
using SimpleBlogApp.Core.Interfaces;
using System;

namespace SimpleBlogApp.Tests.FakeDependencies
{
	class FakeUnitOfWork
	{
		public IUnitOfWork Object { get { return mockUnitOfWork.Object; } }
		private readonly Mock<IUnitOfWork> mockUnitOfWork;

		public FakeUnitOfWork()
		{
			mockUnitOfWork = new Mock<IUnitOfWork>();
		}

		public void SetupSaveAsyncFail()
		{
			mockUnitOfWork.Setup(uof => uof.SaveAsync()).Callback(() => {
				throw new DbUpdateException("test update fail", new Exception());
			});
		}

		public void VerifySave()
		{
			mockUnitOfWork.Verify(u => u.SaveAsync(), Times.AtLeastOnce());
		}
	}
}
