using SimpleBlogApp.Core.Interfaces;
using SimpleBlogApp.Services.Interfaces;
using System.Threading.Tasks;

namespace SimpleBlogApp.Services
{
	public class UnitOfWorkService : IUnitOfWorkService
	{
		private readonly IUnitOfWork unitOfWork;

		public UnitOfWorkService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public async Task<bool> TrySaveChangesAsync()
		{
			try
			{
				await unitOfWork.SaveAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
