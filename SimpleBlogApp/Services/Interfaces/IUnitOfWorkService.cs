using System.Threading.Tasks;

namespace SimpleBlogApp.Services.Interfaces
{
	public interface IUnitOfWorkService
	{
		Task<bool> TrySaveChangesAsync();
	}
}
